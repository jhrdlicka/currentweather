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
using currentweather.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace currentweather.Controllers
{
    [AllowAnonymous]
    //    [Authorize(Policy = "PCMUsersOnly")]
    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class pcm_paymentController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public pcm_paymentController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "pcm_payment";
        }

        // GET: api/pcm_payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_payment>>> Getpcm_payment()
        {
            return await _context.pcm_payment.ToListAsync();
        }

        // GET: api/pcm_payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_payment>> Getpcm_payment(long id)
        {
            var pcm_payment = await _context.pcm_payment.FindAsync(id);

            if (pcm_payment == null)
            {
                return NotFound();
            }

            return pcm_payment;
        }


        // GET: api/pcm_payment/invoiceid/5
        [HttpGet("invoiceid/{id}")]
        public async Task<ActionResult<IEnumerable<pcm_payment>>> Getpcm_payment_invoiceid(long id)
        {
            return await _context.pcm_payment
                .Where(p => p.invoiceid == id)
                .ToListAsync();
        }


        // PUT: api/pcm_payment/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_payment(long id, pcm_payment pcm_payment)
        {
            if (id != pcm_payment.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_paymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return NoContent();
        }

        // POST: api/pcm_payment
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<pcm_payment>> Postpcm_payment(pcm_payment pcm_payment)
        {
            _context.pcm_payment.Add(pcm_payment);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, pcm_payment.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getpcm_payment", new { id = pcm_payment.id }, pcm_payment);
        }

        // DELETE: api/pcm_payment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<pcm_payment>> Deletepcm_payment(long id)
        {
            var pcm_payment = await _context.pcm_payment.FindAsync(id);
            if (pcm_payment == null)
            {
                return NotFound();
            }

            _context.pcm_payment.Remove(pcm_payment);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return pcm_payment;
        }

        private bool pcm_paymentExists(long id)
        {
            return _context.pcm_payment.Any(e => e.id == id);
        }
    }
}
