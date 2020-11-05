using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace currentweather.Controllers
{
    [EnableCors]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthorizeController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var email = this.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            var name = this.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

            return Ok($"Hrdličky zdraví pana '{name}' ({email})");
        }
    }
}
