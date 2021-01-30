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
    public class iot_sampleController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity; 

        public iot_sampleController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_sample";
        }

        // GET: api/iot_sample
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample()
        {
            return await _context.iot_sample.ToListAsync();
        }

        // GET: api/iot_sample/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_sample>> Getiot_sample(long id)
        {
            var iot_sample = await _context.iot_sample.FindAsync(id);

            if (iot_sample == null)
            {
                return NotFound();
            }

            return iot_sample;
        }

        // GET: api/iot_sample/devicecodedate/DEVICE01/2021-01-17/2199-12-31
        [HttpGet("devicecodedate/{code}/{fromdate}/{todate}")]
        public async Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample_devicecodedate(String code, String fromdate, String todate)
        {
            var samples = _context.iot_sample
                    .Where(d => d.device.code == code)
                    .Where(d => string.Compare(d.calendarday.date, fromdate) >= 0)
                    .Where(d => string.Compare(d.calendarday.date, todate) <= 0);                    ;

            return await samples.ToListAsync();
        }

        // PUT: api/iot_sample/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putiot_sample(long id, iot_sample iot_sample)
        {
            if (id != iot_sample.id)
            {
                return BadRequest();
            }

            _context.Entry(iot_sample).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!iot_sampleExists(id))
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

        // POST: api/iot_sample
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_sample>> Postiot_sample(iot_sample iot_sample)
        {
            // if timestamp is not provided, default it to Now
            if (iot_sample.timestamp == DateTime.MinValue)                
                iot_sample.timestamp = DateTime.Now;

            // calculate or validate calendarday
            String lTimestampDay = iot_sample.timestamp.ToString("yyyy-MM-dd");

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
            if (iot_sample.calendardayid == null)
                iot_sample.calendardayid = iot_calendarday.id;
            else
                if (iot_sample.calendardayid != iot_calendarday.id)
                return BadRequest();

            _context.iot_sample.Add(iot_sample);
            await _context.SaveChangesAsync();

            iot_sample.device = null;
            iot_sample.calendarday = null;

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_sample.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return CreatedAtAction("Getiot_sample", new { id = iot_sample.id }, iot_sample);
        }


        // POST: api/iot_sample/devicecode/DEVICE01
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("devicecode/{pDeviceCode}")]
        public async Task<ActionResult<iot_sample>> Postiot_sample_devicecode(string pDeviceCode, iot_sample iot_sample)
        {
            // find device
            if (pDeviceCode == null)
                return NotFound();

            var iot_device = await _context.iot_device.Where(d => d.code == pDeviceCode).FirstOrDefaultAsync();

            if (iot_device == null)
                return NotFound();

            if (iot_sample.deviceid == null)
              iot_sample.deviceid = iot_device.id;
            else
                if (iot_sample.deviceid != iot_device.id)
                return BadRequest();

            iot_sample.device = null;

            return await Postiot_sample(iot_sample);
            /*
            var lIot_sample = await Postiot_sample(iot_sample);
            var lIot_Sample2 = new iot_sample { id = iot_sample.id};
            lIot_Sample2.deviceid = iot_sample.deviceid;
            lIot_Sample2.calendardayid = iot_sample.calendardayid;
            lIot_Sample2.timestamp = iot_sample.timestamp;
            lIot_Sample2.value = iot_sample.value;
            return Created("", lIot_Sample2);
            */
        }


        // DELETE: api/iot_sample/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_sample>> Deleteiot_sample(long id)
        {
            var iot_sample = await _context.iot_sample.FindAsync(id);
            if (iot_sample == null)
            {
                return NotFound();
            }

            _context.iot_sample.Remove(iot_sample);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_sample;
        }

        private bool iot_sampleExists(long id)
        {
            return _context.iot_sample.Any(e => e.id == id);
        }
    }
}
