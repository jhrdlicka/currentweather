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
    public class Pcm_OrderController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public Pcm_OrderController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/Pcm_Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pcm_Order>>> GetPcmOrderTb()
        {
            return await _context.PcmOrderTb.ToListAsync();
        }

        // GET: api/Pcm_Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pcm_Order>> GetPcm_Order(long id)
        {
            var pcm_Order = await _context.PcmOrderTb.FindAsync(id);

            if (pcm_Order == null)
            {
                return NotFound();
            }

            return pcm_Order;
        }

        // PUT: api/Pcm_Order/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcm_Order(long id, Pcm_Order pcm_Order)
        {
            if (id != pcm_Order.Id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Pcm_OrderExists(id))
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

        // POST: api/Pcm_Order
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pcm_Order>> PostPcm_Order(Pcm_Order pcm_Order)
        {
            _context.PcmOrderTb.Add(pcm_Order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Pcm_OrderExists(pcm_Order.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPcm_Order", new { id = pcm_Order.Id }, pcm_Order);
        }

        // DELETE: api/Pcm_Order/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pcm_Order>> DeletePcm_Order(long id)
        {
            var pcm_Order = await _context.PcmOrderTb.FindAsync(id);
            if (pcm_Order == null)
            {
                return NotFound();
            }

            _context.PcmOrderTb.Remove(pcm_Order);
            await _context.SaveChangesAsync();

            return pcm_Order;
        }

        private bool Pcm_OrderExists(long id)
        {
            return _context.PcmOrderTb.Any(e => e.Id == id);
        }
    }
}
