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
    //    [Authorize(Policy = "AllowedUsersOnly")]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class Pcm_CalEventController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public Pcm_CalEventController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/Pcm_CalEvent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pcm_CalEvent>>> GetPcmCalEventTb()
        {
            return await _context.PcmCalEventTb.ToListAsync();
        }

        // GET: api/Pcm_CalEvent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pcm_CalEvent>> GetPcm_CalEvent(long id)
        {
            var pcm_CalEvent = await _context.PcmCalEventTb.FindAsync(id);

            if (pcm_CalEvent == null)
            {
                return NotFound();
            }

            return pcm_CalEvent;
        }

        // PUT: api/Pcm_CalEvent/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcm_CalEvent(long id, Pcm_CalEvent pcm_CalEvent)
        {
            if (id != pcm_CalEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_CalEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Pcm_CalEventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pcm_CalEvent
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pcm_CalEvent>> PostPcm_CalEvent(Pcm_CalEvent pcm_CalEvent)
        {
            _context.PcmCalEventTb.Add(pcm_CalEvent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Pcm_CalEventExists(pcm_CalEvent.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPcm_CalEvent", new { id = pcm_CalEvent.Id }, pcm_CalEvent);
        }

        // DELETE: api/Pcm_CalEvent/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pcm_CalEvent>> DeletePcm_CalEvent(long id)
        {
            var pcm_CalEvent = await _context.PcmCalEventTb.FindAsync(id);
            if (pcm_CalEvent == null)
            {
                return NotFound();
            }

            _context.PcmCalEventTb.Remove(pcm_CalEvent);
            await _context.SaveChangesAsync();

            return pcm_CalEvent;
        }

        private bool Pcm_CalEventExists(long id)
        {
            return _context.PcmCalEventTb.Any(e => e.Id == id);
        }
    }
}
