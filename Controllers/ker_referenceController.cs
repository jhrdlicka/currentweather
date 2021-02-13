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
    public class ker_referenceController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public ker_referenceController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "ker_reference";
        }

        // GET: api/ker_reference
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ker_reference>>> Getker_reference()
        {
            return await _context.ker_reference.ToListAsync();
        }

        // GET: api/ker_reference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ker_reference>> Getker_reference(long id)
        {
            var ker_reference = await _context.ker_reference.FindAsync(id);

            if (ker_reference == null)
            {
                return NotFound();
            }

            return ker_reference;
        }

        // GET: api/ker_reference/reftabnm/5
        [HttpGet("reftabnm/{reftabnm}")]
        public async Task<ActionResult<IEnumerable<ker_reference>>> Getker_reference_reftabnm(string reftabnm)
        {
            return await _context.ker_reference
                .Where(e => e.reftabnm == reftabnm)
                .ToListAsync();
        }


        // PUT: api/ker_reference/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putker_reference(long id, ker_reference ker_reference)
        {
            if (id != ker_reference.id)
            {
                return BadRequest();
            }

            _context.Entry(ker_reference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ker_referenceExists(id))
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

        // POST: api/ker_reference
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ker_reference>> Postker_reference(ker_reference ker_reference)
        {
            _context.ker_reference.Add(ker_reference);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, ker_reference.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getker_reference", new { id = ker_reference.id }, ker_reference);
        }

        // DELETE: api/ker_reference/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ker_reference>> Deleteker_reference(long id)
        {
            var ker_reference = await _context.ker_reference.FindAsync(id);
            if (ker_reference == null)
            {
                return NotFound();
            }

            _context.ker_reference.Remove(ker_reference);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return ker_reference;
        }

        private bool ker_referenceExists(long id)
        {
            return _context.ker_reference.Any(e => e.id == id);
        }
    }
}
