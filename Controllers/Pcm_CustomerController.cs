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
    public class Pcm_CustomerController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public Pcm_CustomerController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/Pcm_Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pcm_Customer>>> GetPcmCustomerTb()
        {
            return await _context.PcmCustomerTb.ToListAsync();
        }

        // GET: api/Pcm_Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pcm_Customer>> GetPcm_Customer(long id)
        {
            var pcm_Customer = await _context.PcmCustomerTb.FindAsync(id);

            if (pcm_Customer == null)
            {
                return NotFound();
            }

            return pcm_Customer;
        }

        // PUT: api/Pcm_Customer/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcm_Customer(long id, Pcm_Customer pcm_Customer)
        {
            if (id != pcm_Customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_Customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Pcm_CustomerExists(id))
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

        // POST: api/Pcm_Customer
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pcm_Customer>> PostPcm_Customer(Pcm_Customer pcm_Customer)
        {
            _context.PcmCustomerTb.Add(pcm_Customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Pcm_CustomerExists(pcm_Customer.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPcm_Customer", new { id = pcm_Customer.Id }, pcm_Customer);
        }

        // DELETE: api/Pcm_Customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pcm_Customer>> DeletePcm_Customer(long id)
        {
            var pcm_Customer = await _context.PcmCustomerTb.FindAsync(id);
            if (pcm_Customer == null)
            {
                return NotFound();
            }

            _context.PcmCustomerTb.Remove(pcm_Customer);
            await _context.SaveChangesAsync();

            return pcm_Customer;
        }

        private bool Pcm_CustomerExists(long id)
        {
            return _context.PcmCustomerTb.Any(e => e.Id == id);
        }
    }
}
