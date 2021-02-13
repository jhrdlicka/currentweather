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
using currentweather.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace currentweather.Controllers
{
    [AllowAnonymous]
    //    [Authorize(Policy = "PCMUsersOnly")]
    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class iot_weatherforecastController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public iot_weatherforecastController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_weatherforecast";
        }

        // GET: api/iot_weatherforecast
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_weatherforecast>>> Getiot_weatherforecast()
        {
            return await _context.iot_weatherforecast.ToListAsync();
        }

        // GET: api/iot_weatherforecast/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_weatherforecast>> Getiot_weatherforecast(long id)
        {
            var iot_weatherforecast = await _context.iot_weatherforecast.FindAsync(id);

            if (iot_weatherforecast == null)
            {
                return NotFound();
            }

            return iot_weatherforecast;
        }

        // PUT: api/iot_weatherforecast/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putiot_weatherforecast(long id, iot_weatherforecast iot_weatherforecast)
        {
            if (id != iot_weatherforecast.id)
            {
                return BadRequest();
            }

            _context.Entry(iot_weatherforecast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!iot_weatherforecastExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return NoContent();
        }

        // POST: api/iot_weatherforecast
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_weatherforecast>> Postiot_weatherforecast(iot_weatherforecast iot_weatherforecast)
        {
            _context.iot_weatherforecast.Add(iot_weatherforecast);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_weatherforecast.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getiot_weatherforecast", new { id = iot_weatherforecast.id }, iot_weatherforecast);
        }

        // DELETE: api/iot_weatherforecast/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_weatherforecast>> Deleteiot_weatherforecast(long id)
        {
            var iot_weatherforecast = await _context.iot_weatherforecast.FindAsync(id);
            if (iot_weatherforecast == null)
            {
                return NotFound();
            }

            _context.iot_weatherforecast.Remove(iot_weatherforecast);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_weatherforecast;
        }

        private bool iot_weatherforecastExists(long id)
        {
            return _context.iot_weatherforecast.Any(e => e.id == id);
        }
    }
}
