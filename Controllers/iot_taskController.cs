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
    //    [Authorize(Policy = "PCMUsersOnly")]
    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class iot_taskController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public iot_taskController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/iot_task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_task>>> Getiot_task()
        {
            return await _context.iot_task.ToListAsync();
        }

        // GET: api/iot_task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_task>> Getiot_task(long id)
        {
            var iot_task = await _context.iot_task.FindAsync(id);

            if (iot_task == null)
            {
                return NotFound();
            }

            return iot_task;
        }

        // PUT: api/iot_task/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putiot_task(long id, iot_task iot_task)
        {
            if (id != iot_task.id)
            {
                return BadRequest();
            }

            _context.Entry(iot_task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!iot_taskExists(id))
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

        // POST: api/iot_task
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_task>> Postiot_task(iot_task iot_task)
        {
            _context.iot_task.Add(iot_task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getiot_task", new { id = iot_task.id }, iot_task);
        }

        // DELETE: api/iot_task/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_task>> Deleteiot_task(long id)
        {
            var iot_task = await _context.iot_task.FindAsync(id);
            if (iot_task == null)
            {
                return NotFound();
            }

            _context.iot_task.Remove(iot_task);
            await _context.SaveChangesAsync();

            return iot_task;
        }

        private bool iot_taskExists(long id)
        {
            return _context.iot_task.Any(e => e.id == id);
        }
    }
}
