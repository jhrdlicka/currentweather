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
    public class pcm_caleventController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public pcm_caleventController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "pcm_calevent";
        }

        // GET: api/pcm_calevent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_calevent>>> Getpcm_calevent()
        {
            //            return await _context.pcm_calevent.Include(d => d.customer).ToListAsync();
            return await _context.pcm_calevent
//                                                .Include(d => d.customer)
                                                .ToListAsync();
        }

        // GET: api/pcm_calevent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_calevent>> Getpcm_calevent(long id)
        {
                        var pcm_calevent = await _context.pcm_calevent.FindAsync(id);
//            var pcm_calevent = await _context.pcm_calevent
//                                                .Include(d => d.customer)
//                                                .SingleOrDefaultAsync(d => d.id == id);                                                

            if (pcm_calevent == null)
            {
                return NotFound();
            }

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return pcm_calevent;
        }

        // PUT: api/pcm_calevent/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_calevent(long id, pcm_calevent pcm_calevent)
        {
            if (id != pcm_calevent.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_calevent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_caleventExists(id))
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

        // POST: api/pcm_calevent
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<pcm_calevent>> Postpcm_calevent(pcm_calevent pcm_calevent)
        {
            _context.pcm_calevent.Add(pcm_calevent);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, pcm_calevent.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getpcm_calevent", new { id = pcm_calevent.id }, pcm_calevent);
        }

        // DELETE: api/pcm_calevent/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<pcm_calevent>> Deletepcm_calevent(long id)
        {
            var pcm_calevent = await _context.pcm_calevent.FindAsync(id);
            if (pcm_calevent == null)
            {
                return NotFound();
            }

            _context.pcm_calevent.Remove(pcm_calevent);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return pcm_calevent;
        }

        private bool pcm_caleventExists(long id)
        {
            return _context.pcm_calevent.Any(e => e.id == id);
        }
    }
}
