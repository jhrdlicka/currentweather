
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using currentweather.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace currentweather.Controllers
{
    [AllowAnonymous]
//    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    // [ApiController]
    public class ExtLoginController : ControllerBase
    {

        /*
        // POST: api/Sensors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Sensor>> PostSensor(Sensor sensor)
        {
            _context.Sensor.Add(sensor);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetSensor", new { id = sensor.Id }, sensor);
            return CreatedAtAction(nameof(GetSensor), new { id = sensor.Id }, sensor);
        }
        */


        [HttpPost]
        public ActionResult ExtLogin(String token)
        {

            
            /*

            var mgr = new Cleaners.Models.UserManager(@"Data Source=.\sqlexpress;Initial Catalog='Cleaning Lady';Integrated Security=True");
            var user = mgr.GetUser(username, password);

            if (user == null)
            {
                return View(new UserViewModel { Name = username });
            }

            FormsAuthentication.SetAuthCookie(user.UserName, true);
            UserViewModel.IsAuthenticated = User.Identity.IsAuthenticated;
            */
            return RedirectToAction("Private");
        }

        [Authorize]
        public ActionResult Private()
        {
            return null;

        }

    }
}