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
    public class iot_calendardayRepository : Iiot_calendardayRepository
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public iot_calendardayRepository(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_calendarday";

        }

        public async Task<ActionResult<IEnumerable<iot_calendarday>>> Getiot_calendarday()
        {
            return await _context.iot_calendarday.ToListAsync();
        }

        public Task<bool> Getiot_calendarday(long id, out iot_calendarday iot_calendarday)
        {

            iot_calendarday = _context.iot_calendarday.SingleOrDefault(x => x.id == id);

            return Task.FromResult(iot_calendarday != null);
        }

        public async Task<ActionResult<iot_calendarday>> Putiot_calendarday(long id, iot_calendarday iot_calendarday)
        {
            
            if (id != iot_calendarday.id)
            {
                //return BadRequest();
            }
            

            _context.Entry(iot_calendarday).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!iot_calendardayExists(id))
                {
                    //                    return NotFound();
                    throw;
                }
                else
                {
                    throw;
                }
            }

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_calendarday;
        }

        public async Task<iot_calendarday> Postiot_calendarday(iot_calendarday iot_calendarday)
        {
            _context.iot_calendarday.Add(iot_calendarday);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_calendarday.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

//          return CreatedAtAction("Getiot_calendarday", new { id = iot_calendarday.id }, iot_calendarday);

            return iot_calendarday;

        }

        public async Task<iot_calendarday> Getiot_calendarday_getorcreatebydate(String pDate)
        {
            var iot_calendarday = await _context.iot_calendarday
                .Where(e => e.date == pDate)
                .FirstOrDefaultAsync();

            if (iot_calendarday == null)
            {
                var new_calendarday = new iot_calendarday { date = pDate };
                return await Postiot_calendarday(new_calendarday);
            }
            else
                return iot_calendarday;
//                return Ok(iot_calendarday);
        }


        public async Task<ActionResult<iot_calendarday>> Deleteiot_calendarday(long id)
        {
            var iot_calendarday = await _context.iot_calendarday.FindAsync(id);
            if (iot_calendarday == null)
            {
//                return NotFound();
            }

            _context.iot_calendarday.Remove(iot_calendarday);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_calendarday;
        }

        public bool iot_calendardayExists(long id)
        {
            return _context.iot_calendarday.Any(e => e.id == id);
        }
    }
}
