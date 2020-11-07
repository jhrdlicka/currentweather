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
    public class Pcm_PaymentController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public Pcm_PaymentController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/Pcm_Payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pcm_Payment>>> GetPcmPaymentTb()
        {
            return await _context.PcmPaymentTb.ToListAsync();
        }

        // GET: api/Pcm_Payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pcm_Payment>> GetPcm_Payment(long id)
        {
            var pcm_Payment = await _context.PcmPaymentTb.FindAsync(id);

            if (pcm_Payment == null)
            {
                return NotFound();
            }

            return pcm_Payment;
        }

        // PUT: api/Pcm_Payment/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcm_Payment(long id, Pcm_Payment pcm_Payment)
        {
            if (id != pcm_Payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_Payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Pcm_PaymentExists(id))
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

        // POST: api/Pcm_Payment
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pcm_Payment>> PostPcm_Payment(Pcm_Payment pcm_Payment)
        {
            _context.PcmPaymentTb.Add(pcm_Payment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Pcm_PaymentExists(pcm_Payment.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPcm_Payment", new { id = pcm_Payment.Id }, pcm_Payment);
        }

        // DELETE: api/Pcm_Payment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pcm_Payment>> DeletePcm_Payment(long id)
        {
            var pcm_Payment = await _context.PcmPaymentTb.FindAsync(id);
            if (pcm_Payment == null)
            {
                return NotFound();
            }

            _context.PcmPaymentTb.Remove(pcm_Payment);
            await _context.SaveChangesAsync();

            return pcm_Payment;
        }

        private bool Pcm_PaymentExists(long id)
        {
            return _context.PcmPaymentTb.Any(e => e.Id == id);
        }
    }
}
