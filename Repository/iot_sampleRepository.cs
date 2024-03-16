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
    public class iot_sampleRepository : Iiot_sampleRepository
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;
        private Iiot_calendardayRepository _iot_calendardayRepository;

        public iot_sampleRepository(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_sample";
            _iot_calendardayRepository = new iot_calendardayRepository(_context, _hubContext);
        }

        public async Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample()
        {
            String lToday = DateTime.UtcNow.ToString("yyyy-MM-dd");
            return await _context.iot_sample.Where(d=>d.calendarday.date == lToday).ToListAsync();
        }

        public async Task<ActionResult<iot_sample>> Getiot_sample(long id)
        {
            var iot_sample = await _context.iot_sample.FindAsync(id);

            return iot_sample;
        }

        public async Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample_devicecodedate(String code, String fromdate, String todate)
        {
            var samples = _context.iot_sample
                    .Where(d => d.device.code == code)
                    .Where(d => string.Compare(d.calendarday.date, fromdate) >= 0)
                    .Where(d => string.Compare(d.calendarday.date, todate) <= 0);

            return await samples.ToListAsync();
        }

        public async Task<IActionResult> Putiot_sample(long id, iot_sample iot_sample)
        {
            if (id != iot_sample.id)
            {
                throw new ArgumentException("Provided object does not fit the ID",
                                  nameof(id));
                //return BadRequest();
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

        public async Task<int> Calcimportancetodayall()
        {            
           String lToday = DateTime.UtcNow.ToString("yyyy-MM-dd");
           return await Calcimportanceall(lToday, lToday,999999);                
        }

        public async Task<int> Calcimportanceall(String fromdate, String todate, int maxtochange)
        {
            var devices = _context.iot_device.ToList();
            int lChanged = 0;
            for (int i = 0; (i < devices.Count)&&(lChanged < maxtochange); i++)
            {
                // test if there are any noncalculated samples
                /*
                var sample = _context.iot_sample
                    .Where(d => d.device.code == devices[i].code)
                    .Where(d => string.Compare(d.calendarday.date, fromdate) >= 0)
                    .Where(d => string.Compare(d.calendarday.date, todate) <= 0)
                    .Where(d => d.importance==null)
                    .FirstOrDefault();
                */
                string sql = $"select TOP(1) s.* from iot_sample s, iot_calendarday c where s.calendardayid=c.id and s.deviceid="+devices[i].id.ToString()+" and c.[date]>='"+fromdate+ "' and c.[date]<='" + fromdate + "' and s.importance is null";
                var samples = _context.iot_sample.FromSqlRaw(sql).ToList();

                Thread.Sleep(30);

                if (samples.Count>0)
                {
                    var lLastChanged = await Calcimportance(devices[i].code, fromdate, todate, maxtochange - lChanged);
                    lChanged = lChanged + lLastChanged;
                }
            }
            return lChanged;
        }

        public async Task<int> Calcimportanceoldest(int maxtochange)
        {
            string sql = $"select * from iot_calendarday where [date] in (select min(mindate) from(select b.date mindate from iot_sample a, iot_calendarday b where a.importance is null and a.calendardayid = b.id group by b.date, a.deviceid having count(1) > 20) x)";
            var oldestcalendarday = _context.iot_calendarday.FromSqlRaw(sql).ToList();
            int lChanged = 0;           
            for (int i = 0; ((i < oldestcalendarday.Count)&&(i<1)); i++)
            {
                var lLastChanged = await Calcimportanceall(oldestcalendarday[i].date, oldestcalendarday[i].date, maxtochange);
                lChanged =lChanged+lLastChanged;
            }
            return lChanged;
        }

        public async Task<int> Calcimportance(String code, String fromdate, String todate, int maxtochange)
        {
            const int MAXIMPORTANCE= 9;
            decimal lMyValue, lMidValue;
            decimal lDiff;
            int lImportance;
            TimeSpan lTimespanBefore, lTimespanAfter;
            decimal lPct1, lPct2, lPct3, lPct4;
            int lChanged = 0;

            var samples = _context.iot_sample
                .Where(d => d.device.code == code)
                .Where(d => string.Compare(d.calendarday.date, fromdate) >= 0)
                .Where(d => string.Compare(d.calendarday.date, todate) <= 0)
                .OrderBy(d => d.timestamp)
                .ToList();

            if (samples.Count == 0)
                return 0;
            //                return NoContent();

            int lSleep = 30;

            if ((samples[0].importance??0) != 9)
            {
                samples[0].importance = 9;
                lChanged++;
                await Putiot_sample(samples[0].id, samples[0]);
                Thread.Sleep(lSleep);
            }

            if ((samples[samples.Count-1].importance ?? 0) != 9)
            {
                samples[samples.Count-1].importance = 9;
                lChanged++;
                await Putiot_sample(samples[samples.Count-1].id, samples[samples.Count-1]);
                Thread.Sleep(lSleep);
            }

            for (int i = 1; (i < samples.Count-1) &&  (lChanged < maxtochange); i++)
            {


                lMyValue = samples[i].value ?? 0;
                lImportance = samples[i].importance??0;

                if (lImportance == 0)
                    lImportance = 0;

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
                        lImportance = Math.Max(lImportance, 1);
                        lImportance = Math.Min(lImportance, 9);
                    }
                }


                if (lImportance != (samples[i].importance??0))
                {
                    lChanged++;
                    samples[i].importance = lImportance;
                    await Putiot_sample(samples[i].id, samples[i]);
                    Thread.Sleep(lSleep);
                }

            }

            return lChanged;
        }


        public async Task<ActionResult<iot_sample>> Postiot_sample(iot_sample iot_sample)
        {
            // if timestamp is not provided, default it to Now
            if (iot_sample.timestamp == DateTime.MinValue)                
                iot_sample.timestamp = DateTime.UtcNow;

            // calculate or validate calendarday
            String lTimestampDay = iot_sample.timestamp.ToString("yyyy-MM-dd");

            var iot_calendarday = await _iot_calendardayRepository.Getiot_calendarday_getorcreatebydate(lTimestampDay);

            if (iot_calendarday == null)
                throw new InvalidOperationException("Calendar day creation failed");
            if (iot_sample.calendardayid == null)
                iot_sample.calendardayid = iot_calendarday.id;
            else
                if (iot_sample.calendardayid != iot_calendarday.id)
                    throw new ArgumentException("Provided calendardayid does not fit the timestamp ("+ lTimestampDay + ")", nameof(iot_calendarday.id));


            _context.iot_sample.Add(iot_sample);
            await _context.SaveChangesAsync();

            iot_sample.device = null;
            iot_sample.calendarday = null;

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_sample.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            //return CreatedAtAction("Postiot_sample", new { id = iot_sample.id }, iot_sample);
            return iot_sample;
        }


        public async Task<ActionResult<iot_sample>> Postiot_sample_devicecode(string pDeviceCode, iot_sample iot_sample)
        {
            // find device
            if (pDeviceCode == null)
                return null;
                //return NotFound();

            var iot_device = await _context.iot_device.Where(d => d.code == pDeviceCode).FirstOrDefaultAsync();

            if (iot_device == null)
                return null;
                //return NotFound();

            if (iot_sample.deviceid == null)
              iot_sample.deviceid = iot_device.id;
            else
                if (iot_sample.deviceid != iot_device.id)
                    throw new ArgumentException("Provided object does not fit the ID", nameof(iot_device.id));
                    //return BadRequest();

            iot_sample.device = null;

            return await Postiot_sample(iot_sample);
        }


        public async Task<ActionResult<iot_sample>> Deleteiot_sample(long id)
        {
            var iot_sample = await _context.iot_sample.FindAsync(id);
            if (iot_sample == null)
            {
                throw new InvalidOperationException("Record (" + _entity + ") not found");
                //return NotFound();
            }

            _context.iot_sample.Remove(iot_sample);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_sample;
        }

        public bool iot_sampleExists(long id)
        {
            return _context.iot_sample.Any(e => e.id == id);
        }
    }
}
