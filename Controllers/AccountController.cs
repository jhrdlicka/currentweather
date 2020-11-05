using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace currentweather.Controllers
{
    [EnableCors]
    [AllowAnonymous, Route("account")]
    public class AccountController : Controller
    {
        static string _redirectUrl = null;

        [Route("google-login")]
        public IActionResult GoogleLogin(string redirectUrl = null)
        {
            _redirectUrl = redirectUrl;

            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest();

            var token = this.Request.Cookies.FirstOrDefault(x => x.Key == ".AspNetCore.Cookies").Value;

            if (string.IsNullOrEmpty(_redirectUrl))
            {
                return Json(new
                {
                    Email = result.Principal.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value,
                    Name = result.Principal.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value,
                    Token = token
                });
            }
            else
            {
                return Redirect($"{_redirectUrl}?token={token}");
            }

        }
    }
}
