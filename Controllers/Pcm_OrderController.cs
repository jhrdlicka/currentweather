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
    public class pcm_orderController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public pcm_orderController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "pcm_order";
        }

        // GET: api/pcm_order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_order>>> Getpcm_order()
        {
            return await _context.pcm_order.ToListAsync();
        }

        // GET: api/pcm_order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_order>> Getpcm_order(long id)
        {
            var pcm_order = await _context.pcm_order.FindAsync(id);

            if (pcm_order == null)
            {
                return NotFound();
            }

            return pcm_order;
        }

        // PUT: api/pcm_order/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_order(long id, pcm_order pcm_order)
        {
            if (id != pcm_order.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_orderExists(id))
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

        // POST: api/pcm_order
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<pcm_order>> Postpcm_order(pcm_order pcm_order)
        {
            _context.pcm_order.Add(pcm_order);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, pcm_order.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getpcm_order", new { id = pcm_order.id }, pcm_order);
        }

        // DELETE: api/pcm_order/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<pcm_order>> Deletepcm_order(long id)
        {
            var pcm_order = await _context.pcm_order.FindAsync(id);
            if (pcm_order == null)
            {
                return NotFound();
            }

            _context.pcm_order.Remove(pcm_order);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return pcm_order;
        }

        private bool pcm_orderExists(long id)
        {
            return _context.pcm_order.Any(e => e.id == id);
        }
    }
}
