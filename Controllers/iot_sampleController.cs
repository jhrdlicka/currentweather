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
    public class iot_sampleController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public iot_sampleController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/iot_sample
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample()
        {
            return await _context.iot_sample.ToListAsync();
        }

        // GET: api/iot_sample/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_sample>> Getiot_sample(long id)
        {
            var iot_sample = await _context.iot_sample.FindAsync(id);

            if (iot_sample == null)
            {
                return NotFound();
            }

            return iot_sample;
        }

        // PUT: api/iot_sample/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putiot_sample(long id, iot_sample iot_sample)
        {
            if (id != iot_sample.id)
            {
                return BadRequest();
            }

            _context.Entry(iot_sample).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!iot_sampleExists(id))
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

        // POST: api/iot_sample
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_sample>> Postiot_sample(iot_sample iot_sample)
        {
            _context.iot_sample.Add(iot_sample);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getiot_sample", new { id = iot_sample.id }, iot_sample);
        }

        // DELETE: api/iot_sample/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_sample>> Deleteiot_sample(long id)
        {
            var iot_sample = await _context.iot_sample.FindAsync(id);
            if (iot_sample == null)
            {
                return NotFound();
            }

            _context.iot_sample.Remove(iot_sample);
            await _context.SaveChangesAsync();

            return iot_sample;
        }

        private bool iot_sampleExists(long id)
        {
            return _context.iot_sample.Any(e => e.id == id);
        }
    }
}
