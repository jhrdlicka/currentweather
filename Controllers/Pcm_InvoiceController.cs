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
    public class Pcm_InvoiceController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public Pcm_InvoiceController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/Pcm_Invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pcm_Invoice>>> GetPcmInvoiceTb()
        {
            return await _context.PcmInvoiceTb.ToListAsync();
        }

        // GET: api/Pcm_Invoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pcm_Invoice>> GetPcm_Invoice(long id)
        {
            var pcm_Invoice = await _context.PcmInvoiceTb.FindAsync(id);

            if (pcm_Invoice == null)
            {
                return NotFound();
            }

            return pcm_Invoice;
        }

        // PUT: api/Pcm_Invoice/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcm_Invoice(long id, Pcm_Invoice pcm_Invoice)
        {
            if (id != pcm_Invoice.Id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_Invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Pcm_InvoiceExists(id))
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

        // POST: api/Pcm_Invoice
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pcm_Invoice>> PostPcm_Invoice(Pcm_Invoice pcm_Invoice)
        {
            _context.PcmInvoiceTb.Add(pcm_Invoice);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Pcm_InvoiceExists(pcm_Invoice.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPcm_Invoice", new { id = pcm_Invoice.Id }, pcm_Invoice);
        }

        // DELETE: api/Pcm_Invoice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pcm_Invoice>> DeletePcm_Invoice(long id)
        {
            var pcm_Invoice = await _context.PcmInvoiceTb.FindAsync(id);
            if (pcm_Invoice == null)
            {
                return NotFound();
            }

            _context.PcmInvoiceTb.Remove(pcm_Invoice);
            await _context.SaveChangesAsync();

            return pcm_Invoice;
        }

        private bool Pcm_InvoiceExists(long id)
        {
            return _context.PcmInvoiceTb.Any(e => e.Id == id);
        }
    }
}
