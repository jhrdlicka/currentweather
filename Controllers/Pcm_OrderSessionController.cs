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
    public class Pcm_OrderSessionController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public Pcm_OrderSessionController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/Pcm_OrderSession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pcm_OrderSession>>> GetPcmOrderSessionTb()
        {
            return await _context.PcmOrderSessionTb.ToListAsync();
        }

        // GET: api/Pcm_OrderSession/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pcm_OrderSession>> GetPcm_OrderSession(long id)
        {
            var pcm_OrderSession = await _context.PcmOrderSessionTb.FindAsync(id);

            if (pcm_OrderSession == null)
            {
                return NotFound();
            }

            return pcm_OrderSession;
        }

        // PUT: api/Pcm_OrderSession/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcm_OrderSession(long id, Pcm_OrderSession pcm_OrderSession)
        {
            if (id != pcm_OrderSession.Id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_OrderSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Pcm_OrderSessionExists(id))
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

        // POST: api/Pcm_OrderSession
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pcm_OrderSession>> PostPcm_OrderSession(Pcm_OrderSession pcm_OrderSession)
        {
            _context.PcmOrderSessionTb.Add(pcm_OrderSession);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Pcm_OrderSessionExists(pcm_OrderSession.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPcm_OrderSession", new { id = pcm_OrderSession.Id }, pcm_OrderSession);
        }

        // DELETE: api/Pcm_OrderSession/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pcm_OrderSession>> DeletePcm_OrderSession(long id)
        {
            var pcm_OrderSession = await _context.PcmOrderSessionTb.FindAsync(id);
            if (pcm_OrderSession == null)
            {
                return NotFound();
            }

            _context.PcmOrderSessionTb.Remove(pcm_OrderSession);
            await _context.SaveChangesAsync();

            return pcm_OrderSession;
        }

        private bool Pcm_OrderSessionExists(long id)
        {
            return _context.PcmOrderSessionTb.Any(e => e.Id == id);
        }
    }
}
