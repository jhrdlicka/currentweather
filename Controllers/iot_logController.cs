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
    public class iot_logController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public iot_logController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_log";
        }

        // GET: api/iot_log
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_log>>> Getiot_log()
        {
            return await _context.iot_log.ToListAsync();
        }

        // GET: api/iot_log/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_log>> Getiot_log(long id)
        {
            var iot_log = await _context.iot_log.FindAsync(id);

            if (iot_log == null)
            {
                return NotFound();
            }

            return iot_log;
        }

        // PUT: api/iot_log/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putiot_log(long id, iot_log iot_log)
        {
            if (id != iot_log.id)
            {
                return BadRequest();
            }

            _context.Entry(iot_log).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!iot_logExists(id))
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

        // POST: api/iot_log
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_log>> Postiot_log(iot_log iot_log)
        {

            // if timestamp is not provided, default it to Now
            if (iot_log.timestamp == DateTime.MinValue)
                iot_log.timestamp = DateTime.UtcNow;

            // calculate or validate calendarday
            String lTimestampDay = iot_log.timestamp.ToString("yyyy-MM-dd");

            // start of hack (as I am not able to call another controller
            //var iot_calendarday = await iot_calendardayController.Getiot_calendarday_getorcreatebydate(lTimestampDay);
            var iot_calendarday = await _context.iot_calendarday.Where(cd => cd.date == lTimestampDay).FirstOrDefaultAsync();
            if (iot_calendarday == null)
            {
                iot_calendarday = new iot_calendarday { date = lTimestampDay };
                _context.iot_calendarday.Add(iot_calendarday);
                await _context.SaveChangesAsync();
                var lMsg2 = new ServerUpdateHubMsg("iot_calendarday", ServerUpdateHubMsg.TOperation.INSERT, iot_calendarday.id);
                var lJson2 = JsonConvert.SerializeObject(lMsg2);
                await _hubContext.Clients.All.SendAsync(lMsg2.entity, lJson2);

            }
            // end of the hack

            if (iot_calendarday == null)
                return NotFound();
            if (iot_log.calendardayid == null)
                iot_log.calendardayid = iot_calendarday.id;
            else
                if (iot_log.calendardayid != iot_calendarday.id)
                return BadRequest();

            _context.iot_log.Add(iot_log);
            await _context.SaveChangesAsync();

            iot_log.device = null;
            iot_log.calendarday = null;

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_log.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getiot_log", new { id = iot_log.id }, iot_log);
        }

        // POST: api/iot_log/devicecode/DEVICE01
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("devicecode/{pDeviceCode}")]
        public async Task<ActionResult<iot_log>> Postiot_log_devicecode(string pDeviceCode, iot_log iot_log)
        {
            // find device
            if (pDeviceCode == null)
                return NotFound();

            var iot_device = await _context.iot_device.Where(d => d.code == pDeviceCode).FirstOrDefaultAsync();

            if (iot_device == null)
                return NotFound();

            if (iot_log.deviceid == null)
                iot_log.deviceid = iot_device.id;
            else
                if (iot_log.deviceid != iot_device.id)
                return BadRequest();

            iot_log.device = null;

            return await Postiot_log(iot_log);
        }

        public class JsonResult
        {
            public long DeletedRows { get; set; }
        };

        // DELETE: api/iot_log/older/2
        [HttpDelete("older/{days}")]
        public async Task<ActionResult<iot_log>> Deleteolderiot_log(int days)
        {
            var lIot_log = _context.iot_log.Where(e => e.timestamp < DateTime.UtcNow.AddDays(-days));
            long lDeletedRows = lIot_log.Count();
            _context.iot_log.RemoveRange(lIot_log);

            await _context.SaveChangesAsync();

/*
            var lLogitems = _context.iot_log.Where(e => e.timestamp < DateTime.UtcNow.AddDays(-days));
            foreach (iot_log lLogitem in lLogitems)
            {
                await Deleteiot_log(lLogitem.id);
            }
*/


            var jsonResult=new JsonResult{DeletedRows=lDeletedRows};

            string returnJson = JsonConvert.SerializeObject(jsonResult);
            return Ok(returnJson);
        }


        // DELETE: api/iot_log/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_log>> Deleteiot_log(long id)
        {
            var iot_log = await _context.iot_log.FindAsync(id);
            if (iot_log == null)
            {
                return NotFound();
            }

            _context.iot_log.Remove(iot_log);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_log;
        }

        private bool iot_logExists(long id)
        {
            return _context.iot_log.Any(e => e.id == id);
        }
    }
}
