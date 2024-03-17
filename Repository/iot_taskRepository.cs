using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using currentweather.Models;
using currentweather.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace currentweather.Repository
{
    public class iot_taskRepository : Iiot_taskRepository
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;
        private Iiot_calendardayRepository _iot_calendardayRepository;

        public iot_taskRepository(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_task";
            _iot_calendardayRepository = new iot_calendardayRepository(_context, _hubContext);
        }

        public async Task<ActionResult<IEnumerable<iot_task>>> Getiot_task()
        {
            return await _context.iot_task.ToListAsync();
        }

        public async Task<ActionResult<iot_task>> Getiot_task(long id)
        {
            var iot_task = await _context.iot_task.FindAsync(id);

            return iot_task;
        }

        public async Task<ActionResult<IEnumerable<iot_task>>> Getiot_task_devicecode(String code)
        {
            var getstatuses = new string[] { "SCHEDULED", "ACCEPTED" };
            var tasks = _context.iot_task
                    .Where(d => d.device.code == code)
                    .Where(d => !d.completed.HasValue)
                    .Where(d => getstatuses.Any(s => d.taskstatusnm == s));

            return await tasks.ToListAsync();
        }

        public async Task<IActionResult> Putiot_task(long id, iot_task iot_task)
        {
            if (id != iot_task.id)
            {
                throw new ArgumentException("Provided object does not fit the ID",
                                  nameof(id));
                //return BadRequest();
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
                    throw new InvalidOperationException("Record ("+_entity+") not found");
                    //return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            //return NoContent();
            return null;
        }

        public Task<IActionResult> Putiot_task_completed(long id, iot_task iot_task)
        {
            iot_task = _context.iot_task.Where(d => d.id == id).FirstOrDefault();
            iot_task.completed = DateTime.UtcNow;
            iot_task.taskstatusnm = "COMPLETED";

            return Putiot_task(id, iot_task);
        }

        public Task<IActionResult> Putiot_task_accepted(long id, iot_task iot_task)
        {
            iot_task = _context.iot_task.Where(d => d.id == id).FirstOrDefault();
            iot_task.accepted = DateTime.UtcNow;
            iot_task.taskstatusnm = "ACCEPTED";

            return Putiot_task(id, iot_task);
        }

        public Task<IActionResult> Putiot_task_failed(long id, iot_task iot_task)
        {

            iot_task = _context.iot_task.Where(d => d.id == id).FirstOrDefault();
            iot_task.completed = DateTime.UtcNow;
            iot_task.taskstatusnm = "FAILED";

            return Putiot_task(id, iot_task);
        }

        public async Task<ActionResult<iot_task>> Postiot_task(iot_task iot_task)
        {
            if (!iot_task.scheduled.HasValue)
                iot_task.scheduled = DateTime.UtcNow;

            _context.iot_task.Add(iot_task);
            await _context.SaveChangesAsync();

            iot_task.device = null;

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_task.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            //return CreatedAtAction("Postiot_task", new { id = iot_task.id }, iot_task);
            return iot_task;
        }


        public async Task<ActionResult<iot_task>> Postiot_task_command(String code, String command)
        {
            var iot_task = new iot_task();

            var device = _context.iot_device.Where(d => d.code == code).FirstOrDefault();

            iot_task.deviceid = device.id;
            iot_task.taskstatusnm = "SCHEDULED";
            iot_task.command = command;

            return await Postiot_task(iot_task);
        }


        public async Task<ActionResult<iot_task>> Postiot_task_devicecode(string pDeviceCode, iot_task iot_task)
        {
            // find device
            if (pDeviceCode == null)
                return null;
                //return NotFound();

            var iot_device = await _context.iot_device.Where(d => d.code == pDeviceCode).FirstOrDefaultAsync();

            if (iot_device == null)
                return null;
                //return NotFound();

            if (iot_task.deviceid == null)
              iot_task.deviceid = iot_device.id;
            else
                if (iot_task.deviceid != iot_device.id)
                    throw new ArgumentException("Provided object does not fit the ID", nameof(iot_device.id));
                    //return BadRequest();

            iot_task.device = null;

            return await Postiot_task(iot_task);
        }


        public async Task<ActionResult<iot_task>> Deleteiot_task(long id)
        {
            var iot_task = await _context.iot_task.FindAsync(id);
            if (iot_task == null)
            {
                throw new InvalidOperationException("Record (" + _entity + ") not found");
                //return NotFound();
            }

            _context.iot_task.Remove(iot_task);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_task;
        }

        public bool iot_taskExists(long id)
        {
            return _context.iot_task.Any(e => e.id == id);
        }
    }
}
