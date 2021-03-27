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
            String lToday = DateTime.Now.ToString("yyyy-MM-dd");
            return await _context.iot_sample.Where(d=>d.calendarday.date == lToday).ToListAsync();
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
                    .Where(d => string.Compare(d.calendarday.date, todate) <= 0);

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

            if (iot_sample.importance == null)
                iot_sample.importance = 0;

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

        // PUT: api/iot_sample/calcimportancetodayall
        [HttpPut("Calcimportancetodayall")]
        public async Task<IActionResult> Calcimportancetodayall()
        {            
            while (true)
            {
                String lToday = DateTime.Now.ToString("yyyy-MM-dd");
                await Calcimportanceall(lToday, lToday);
                await Task.Delay(10000);
            }
        }

        // PUT: api/iot_sample/calcimportanceall/2021-01-17/2199-12-31
        [HttpPut("calcimportanceall/{fromdate}/{todate}")]
        public async Task<IActionResult> Calcimportanceall(String fromdate, String todate)
        {
            var devices = await _context.iot_device.ToListAsync();
            for (int i = 0; i < devices.Count; i++)
            {
                await Calcimportance(devices[i].code, fromdate, todate);
            }
            return NoContent();
        }


        // PUT: api/iot_sample/calcimportance/DEVICE01/2021-01-17/2199-12-31
        [HttpPut("calcimportance/{code}/{fromdate}/{todate}")]
        public async Task<IActionResult> Calcimportance(String code, String fromdate, String todate)
        {
            const int MAXIMPORTANCE= 9;
            decimal lMyValue, lMidValue;
            decimal lDiff;
            int lImportance;
            TimeSpan lTimespanBefore, lTimespanAfter;
            decimal lPct1, lPct2, lPct3, lPct4;             

            var samples = await _context.iot_sample
                .Where(d => d.device.code == code)
                .Where(d => string.Compare(d.calendarday.date, fromdate) >= 0)
                .Where(d => string.Compare(d.calendarday.date, todate) <= 0)
                .OrderBy(d => d.timestamp)
                .ToListAsync();

            if (samples.Count==0)
                return NoContent();

            if ((samples[0].importance??0) != 9)
            {
                samples[0].importance = 9;
                await Putiot_sample(samples[0].id, samples[0]);
            }

            if ((samples[samples.Count-1].importance ?? 0) != 9)
            {
                samples[samples.Count-1].importance = 9;
                await Putiot_sample(samples[samples.Count-1].id, samples[samples.Count-1]);
            }

            for (int i = 1; i < samples.Count-1; i++)
            {

                lMyValue = samples[i].value ?? 0;
                lImportance = samples[i].importance??0;
                if ((samples[i].calendardayid != samples[i - 1].calendardayid) ||
                      (samples[i].calendardayid != samples[i + 1].calendardayid)) // first and last value in the day is always of the highest importance
                    lImportance = MAXIMPORTANCE;
                else
                {
                    lTimespanBefore = samples[i].timestamp - samples[i - 1].timestamp;
                    lTimespanAfter = samples[i + 1].timestamp - samples[i].timestamp;
                    lDiff = (samples[i - 1].value ?? 0) - (samples[i + 1].value ?? 0);            // diff between previous and following values
                    if ((lTimespanBefore.TotalMilliseconds == 0) && (lTimespanAfter.TotalMilliseconds == 0))  // strange situation... shouldn't happen
                        lImportance = 9;
                    else
                    {
                        lMidValue = (samples[i - 1].value ?? 0) + Convert.ToDecimal(Decimal.ToDouble(lDiff) * (lTimespanBefore / (lTimespanAfter + lTimespanBefore)));  // weigthed average value

                        // Pct1 ... difference against average value in pct of the value
                        if ((lMidValue == 0) && (lMyValue == 0))
                            lPct1 = 0;
                        else
                            lPct1 = Math.Abs((lMidValue - lMyValue) / Math.Max(Math.Abs(lMidValue), Math.Abs(lMyValue)));

                        // Pct2 ... difference against average value in pct of the change
                        if (lDiff == 0)
                        {
                            if (lMidValue == lMyValue)
                                lPct2 = 0;
                            else
                                lPct2 = 1;
                        }
                        else
                            lPct2 = Math.Abs((lMidValue - lMyValue) / lDiff);

                        // lPct3 ... how far am I from the previous and next sample
                        lPct3 = (decimal)(Math.Min(lTimespanBefore.TotalMilliseconds, lTimespanAfter.TotalMilliseconds) / (lTimespanBefore.TotalMilliseconds + lTimespanAfter.TotalMilliseconds));

                        // lPct4 ... 
                        int lSamplesDay = samples.Where(x => x.calendardayid == samples[i].calendardayid).ToList().Count;
                        float lAvgDur = 24 * 60 * 60 * 1000 / lSamplesDay;
                        lPct4 = (decimal)((Math.Max(lTimespanBefore.TotalMilliseconds, lTimespanAfter.TotalMilliseconds)) / lAvgDur);

                        lImportance = (int)Math.Round(Math.Min(lPct1 * 3, 5) + Math.Min(lPct2 * 3, 5) + Math.Min(lPct3 * 4, 1) + Math.Min(lPct4 * 1/2, 3));
                        lImportance = Math.Min(lImportance, 9);
                    }
                }


                if (lImportance != (samples[i].importance??0))
                {
                    samples[i].importance = lImportance;
                    await Putiot_sample(samples[i].id, samples[i]);
                }

            }

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
