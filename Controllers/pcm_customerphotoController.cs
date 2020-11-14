using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using currentweather.Models;

namespace currentweather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pcm_customerphotoController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public pcm_customerphotoController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/pcm_customerphoto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_customerphoto>>> Getpcm_customerphoto()
        {
            return await _context.pcm_customerphoto.ToListAsync();
        }

        // GET: api/pcm_customerphoto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_customerphoto>> Getpcm_customerphoto(long id)
        {
            var pcm_customerphoto = await _context.pcm_customerphoto.FindAsync(id);

            if (pcm_customerphoto == null)
            {
                return NotFound();
            }

            return pcm_customerphoto;
        }

        // PUT: api/pcm_customerphoto/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_customerphoto(long id, pcm_customerphoto pcm_customerphoto)
        {
            if (id != pcm_customerphoto.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_customerphoto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_customerphotoExists(id))
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

        private bool pcm_customerphotoExists(long id)
        {
            return _context.pcm_customerphoto.Any(e => e.id == id);
        }
    }
}
