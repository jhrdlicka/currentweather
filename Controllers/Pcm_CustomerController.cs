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
    public class pcm_customerController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public pcm_customerController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "pcm_customer";
        }

        // GET: api/pcm_customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_customer>>> Getpcm_customer()
        {

            return await _context.pcm_customer.ToListAsync();
        }

        // GET: api/pcm_customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_customer>> Getpcm_customer(long id)
        {
            var pcm_customer = await _context.pcm_customer.FindAsync(id);

            if (pcm_customer == null)
            {
                return NotFound();
            }

            return pcm_customer;
        }

        // PUT: api/pcm_customer/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_customer(long id, pcm_customer pcm_customer)
        {
            if (id != pcm_customer.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_customerExists(id))
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

        // POST: api/pcm_customer
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<pcm_customer>> Postpcm_customer(pcm_customer pcm_customer)
        {
            _context.pcm_customer.Add(pcm_customer);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, pcm_customer.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getpcm_customer", new { id = pcm_customer.id }, pcm_customer);
        }

        // DELETE: api/pcm_customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<pcm_customer>> Deletepcm_customer(long id)
        {
            var pcm_customer = await _context.pcm_customer.FindAsync(id);
            if (pcm_customer == null)
            {
                return NotFound();
            }

            _context.pcm_customer.Remove(pcm_customer);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return pcm_customer;
        }

        private bool pcm_customerExists(long id)
        {
            return _context.pcm_customer.Any(e => e.id == id);
        }
    }
}
