using System;
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
    public class iot_logRepository : Iiot_logRepository
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;
        private Iiot_calendardayRepository _iot_calendardayRepository;

        public iot_logRepository(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_log";
            _iot_calendardayRepository = new iot_calendardayRepository(_context, _hubContext); 
        }

        public async Task<ActionResult<IEnumerable<iot_log>>> Getiot_log()
        {
            return await _context.iot_log.ToListAsync();
        }

        public Task<bool> Getiot_log(long id, out iot_log iot_log)
        {

            iot_log = _context.iot_log.SingleOrDefault(x => x.id == id);

            return Task.FromResult(iot_log != null);
        }

        public async Task<IActionResult> Putiot_log(long id, iot_log iot_log)
        {
            if (id != iot_log.id)
            {
                throw new ArgumentException("Provided object does not fit the ID", nameof(id));
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
                    throw new InvalidOperationException("Record (" + _entity + ") not found");
                }
                else
                {
                    throw;
                }
            }

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return null;
        }

        public async Task<ActionResult<iot_log>> Postiot_log(iot_log iot_log)
        {

            // if timestamp is not provided, default it to Now
            if (iot_log.timestamp == DateTime.MinValue)
                iot_log.timestamp = DateTime.UtcNow;

            // calculate or validate calendarday
            String lTimestampDay = iot_log.timestamp.ToString("yyyy-MM-dd");

            var iot_calendarday = await _iot_calendardayRepository.Getiot_calendarday_getorcreatebydate(lTimestampDay);


            if (iot_calendarday == null)
                throw new InvalidOperationException("Calendar day creation failed");
            if (iot_log.calendardayid == null)
                iot_log.calendardayid = iot_calendarday.id;
            else
                if (iot_log.calendardayid != iot_calendarday.id)
                throw new ArgumentException("Provided calendardayid does not fit the timestamp (" + lTimestampDay + ")", nameof(iot_calendarday.id));

            _context.iot_log.Add(iot_log);
            await _context.SaveChangesAsync();

            iot_log.device = null;
            iot_log.calendarday = null;

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_log.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_log;
        }


        public async Task<ActionResult<iot_log>> Postiot_log_devicecode(string pDeviceCode, iot_log iot_log)
        {
            // find device
            if (pDeviceCode == null)
                throw new System.ArgumentNullException("pDeviceCode");

            var iot_device = await _context.iot_device.Where(d => d.code == pDeviceCode).FirstOrDefaultAsync();

            if (iot_device == null)
                throw new InvalidOperationException("Record (iot_device) not found"); 

            if (iot_log.deviceid == null)
                iot_log.deviceid = iot_device.id;
            else
                if (iot_log.deviceid != iot_device.id)
                    throw new ArgumentException("Provided object (iot_device) does not fit the ID", nameof(iot_device.id));

            iot_log.device = null;

            return await Postiot_log(iot_log);
        }

        public class JsonResult
        {
            public long DeletedRows { get; set; }
        };

        public async Task<string> Deleteolderiot_log(int days)
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
            return returnJson;
        }


        public async Task<ActionResult<iot_log>> Deleteiot_log(long id)
        {
            var iot_log = await _context.iot_log.FindAsync(id);
            if (iot_log == null)
            {
                throw new InvalidOperationException("Record (" + _entity + ") not found");
            }

            _context.iot_log.Remove(iot_log);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_log;
        }

        public bool iot_logExists(long id)
        {
            return _context.iot_log.Any(e => e.id == id);
        }
    }
}
