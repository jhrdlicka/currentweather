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
    public class ker_reftabController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public ker_reftabController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/ker_reftab
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ker_reftab>>> Getker_reftab()
        {
            return await _context.ker_reftab.ToListAsync();
        }

        // GET: api/ker_reftab/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ker_reftab>> Getker_reftab(long id)
        {
            var ker_reftab = await _context.ker_reftab.FindAsync(id);

            if (ker_reftab == null)
            {
                return NotFound();
            }

            return ker_reftab;
        }

        // PUT: api/ker_reftab/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putker_reftab(long id, ker_reftab ker_reftab)
        {
            if (id != ker_reftab.id)
            {
                return BadRequest();
            }

            _context.Entry(ker_reftab).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ker_reftabExists(id))
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

        // POST: api/ker_reftab
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ker_reftab>> Postker_reftab(ker_reftab ker_reftab)
        {
            _context.ker_reftab.Add(ker_reftab);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getker_reftab", new { id = ker_reftab.id }, ker_reftab);
        }

        // DELETE: api/ker_reftab/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ker_reftab>> Deleteker_reftab(long id)
        {
            var ker_reftab = await _context.ker_reftab.FindAsync(id);
            if (ker_reftab == null)
            {
                return NotFound();
            }

            _context.ker_reftab.Remove(ker_reftab);
            await _context.SaveChangesAsync();

            return ker_reftab;
        }

        private bool ker_reftabExists(long id)
        {
            return _context.ker_reftab.Any(e => e.id == id);
        }
    }
}
