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
    public class iot_taskController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public iot_taskController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_task";
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

        // GET: api/iot_task/devicecode/DEVICE01
        [HttpGet("devicecode/{code}")]
        public async Task<ActionResult<IEnumerable<iot_task>>> Getiot_task_devicecode(String code)
        {
            var getstatuses = new string[] { "SCHEDULED", "ACCEPTED" };
            var tasks = _context.iot_task
                    .Where(d => d.device.code==code)
                    .Where(d => !d.completed.HasValue)
                    .Where(d => getstatuses.Any(s => d.taskstatusnm==s));

             return await tasks.ToListAsync();
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

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return NoContent();
        }


        // PUT: api/iot_task/completed/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("completed/{id}")]
        public async Task<IActionResult> Putiot_task_completed(long id, iot_task iot_task)
        {
            if (id != iot_task.id)
            {
                return BadRequest();
            }

            _context.Entry(iot_task).State = EntityState.Modified;

            try
            {
                if (iot_task.completed.HasValue)
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE iot_task SET " +
                        "  completed={1}, " +
                        "  taskstatusnm='COMPLETED' " +
                        "  WHERE id={0}",
                        parameters: new[] { iot_task.id.ToString(), iot_task.completed.ToString() });
                else                    
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE iot_task SET " +
                        "  completed=CURRENT_TIMESTAMP, " +
                        "  taskstatusnm='COMPLETED' " +
                        "  WHERE id={0}",
                        parameters: iot_task.id);
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

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return NoContent();
        }

        // PUT: api/iot_task/accepted/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("accepted/{id}")]
        public async Task<IActionResult> Putiot_task_accepted(long id, iot_task iot_task)
        {
            if (id != iot_task.id)
            {
                return BadRequest();
            }

            _context.Entry(iot_task).State = EntityState.Modified;

            try
            {
                if (iot_task.accepted.HasValue)
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE iot_task SET " +
                        "  accepted={1}, " +
                        "  taskstatusnm='ACCEPTED' " +
                        "  WHERE id={0}",
                        parameters: new[] { iot_task.id.ToString(), iot_task.accepted.ToString() });
                else
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE iot_task SET " +
                        "  accepted=CURRENT_TIMESTAMP, " +
                        "  taskstatusnm='ACCEPTED' " +
                        "  WHERE id={0}",
                        parameters: iot_task.id);
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

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return NoContent();
        }

        // PUT: api/iot_task/failed/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("failed/{id}")]
        public async Task<IActionResult> Putiot_task_failed(long id, iot_task iot_task)
        {
            if (id != iot_task.id)
            {
                return BadRequest();
            }

            _context.Entry(iot_task).State = EntityState.Modified;

            try
            {
                if (iot_task.completed.HasValue)
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE iot_task SET " +
                        "  completed={1}, " +
                        "  taskstatusnm='FAILED' " +
                        "  WHERE id={0}",
                        parameters: new[] { iot_task.id.ToString(), iot_task.completed.ToString() });
                else
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE iot_task SET " +
                        "  completed=null, " +
                        "  taskstatusnm='FAILED' " +
                        "  WHERE id={0}",
                        parameters: iot_task.id);
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

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

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

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_task.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

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

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_task;
        }

        private bool iot_taskExists(long id)
        {
            return _context.iot_task.Any(e => e.id == id);
        }
    }
}
