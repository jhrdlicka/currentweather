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
    public class pcm_ordersessionController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public pcm_ordersessionController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/pcm_ordersession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_ordersession>>> Getpcm_ordersession()
        {
            return await _context.pcm_ordersession.ToListAsync();
        }

        // GET: api/pcm_ordersession/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_ordersession>> Getpcm_ordersession(long id)
        {
            var pcm_ordersession = await _context.pcm_ordersession.FindAsync(id);

            if (pcm_ordersession == null)
            {
                return NotFound();
            }

            return pcm_ordersession;
        }

        // PUT: api/pcm_ordersession/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_ordersession(long id, pcm_ordersession pcm_ordersession)
        {
            if (id != pcm_ordersession.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_ordersession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_ordersessionExists(id))
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

        // POST: api/pcm_ordersession
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<pcm_ordersession>> Postpcm_ordersession(pcm_ordersession pcm_ordersession)
        {
            _context.pcm_ordersession.Add(pcm_ordersession);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpcm_ordersession", new { id = pcm_ordersession.id }, pcm_ordersession);
        }

        // DELETE: api/pcm_ordersession/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<pcm_ordersession>> Deletepcm_ordersession(long id)
        {
            var pcm_ordersession = await _context.pcm_ordersession.FindAsync(id);
            if (pcm_ordersession == null)
            {
                return NotFound();
            }

            _context.pcm_ordersession.Remove(pcm_ordersession);
            await _context.SaveChangesAsync();

            return pcm_ordersession;
        }

        private bool pcm_ordersessionExists(long id)
        {
            return _context.pcm_ordersession.Any(e => e.id == id);
        }
    }
}
