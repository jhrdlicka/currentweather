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
    //    [Authorize(Policy = "PCMUsersOnly")]
    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class pcm_gcaleventController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public pcm_gcaleventController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/pcm_gcalevent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_gcalevent>>> Getpcm_gcalevent()
        {
            return await _context.pcm_gcalevent.ToListAsync();
        }

        // GET: api/pcm_gcalevent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_gcalevent>> Getpcm_gcalevent(long id)
        {
            var pcm_gcalevent = await _context.pcm_gcalevent.FindAsync(id);

            if (pcm_gcalevent == null)
            {
                return NotFound();
            }

            return pcm_gcalevent;
        }

        // PUT: api/pcm_gcalevent/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_gcalevent(long id, pcm_gcalevent pcm_gcalevent)
        {
            if (id != pcm_gcalevent.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_gcalevent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_gcaleventExists(id))
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

        // POST: api/pcm_gcalevent
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<pcm_gcalevent>> Postpcm_gcalevent(pcm_gcalevent pcm_gcalevent)
        {
            _context.pcm_gcalevent.Add(pcm_gcalevent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpcm_gcalevent", new { id = pcm_gcalevent.id }, pcm_gcalevent);
        }

        // DELETE: api/pcm_gcalevent/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<pcm_gcalevent>> Deletepcm_gcalevent(long id)
        {
            var pcm_gcalevent = await _context.pcm_gcalevent.FindAsync(id);
            if (pcm_gcalevent == null)
            {
                return NotFound();
            }

            _context.pcm_gcalevent.Remove(pcm_gcalevent);
            await _context.SaveChangesAsync();

            return pcm_gcalevent;
        }

        private bool pcm_gcaleventExists(long id)
        {
            return _context.pcm_gcalevent.Any(e => e.id == id);
        }
    }
}
