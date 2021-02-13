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
    public class pcm_invoiceController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public pcm_invoiceController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "pcm_invoice";
        }

        // GET: api/pcm_invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_invoice>>> Getpcm_invoice()
        {
            return await _context.pcm_invoice.ToListAsync();
        }

        // GET: api/pcm_invoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_invoice>> Getpcm_invoice(long id)
        {
            var pcm_invoice = await _context.pcm_invoice.FindAsync(id);

            if (pcm_invoice == null)
            {
                return NotFound();
            }

            return pcm_invoice;
        }


        // GET: api/pcm_invoice/orderid/5
        [HttpGet("orderid/{id}")]
        public async Task<ActionResult<IEnumerable<pcm_invoice>>> Getpcm_invoice_orderid(long id)
        {
            return await _context.pcm_invoice
                .Where(i => i.orderid == id)
                .ToListAsync();
        }


        // PUT: api/pcm_invoice/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_invoice(long id, pcm_invoice pcm_invoice)
        {
            if (id != pcm_invoice.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_invoiceExists(id))
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

        // POST: api/pcm_invoice
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<pcm_invoice>> Postpcm_invoice(pcm_invoice pcm_invoice)
        {
            _context.pcm_invoice.Add(pcm_invoice);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, pcm_invoice.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getpcm_invoice", new { id = pcm_invoice.id }, pcm_invoice);
        }

        // DELETE: api/pcm_invoice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<pcm_invoice>> Deletepcm_invoice(long id)
        {
            var pcm_invoice = await _context.pcm_invoice.FindAsync(id);
            if (pcm_invoice == null)
            {
                return NotFound();
            }

            _context.pcm_invoice.Remove(pcm_invoice);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return pcm_invoice;
        }

        private bool pcm_invoiceExists(long id)
        {
            return _context.pcm_invoice.Any(e => e.id == id);
        }
    }
}
