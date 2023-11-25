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
    public class iot_deviceRepository : Iiot_deviceRepository
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private readonly string _entity;

        public iot_deviceRepository(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _entity = "iot_device";
        }

        public async Task<ActionResult<IEnumerable<iot_device>>> Getiot_device()
        {
            return await _context.iot_device.ToListAsync();
        }

        // GET: api/iot_device/5
        public async Task<ActionResult<iot_device>> Getiot_device(long id)
        {
            var iot_device = await _context.iot_device.FindAsync(id);

            return iot_device;
        }

        public async Task<ActionResult<iot_device>> Putiot_device(long id, iot_device iot_device)
        {

            _context.Entry(iot_device).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.UPDATE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_device;
        }

        public async Task<ActionResult<iot_device>> Postiot_device(iot_device iot_device)
        {
            _context.iot_device.Add(iot_device);
            await _context.SaveChangesAsync();
            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, iot_device.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_device;
        }


        public async Task<ActionResult<iot_device>> Copyiot_device(long sourceid, long? masterdeviceid)
        {
            var sourcedevice = await _context.iot_device.FindAsync(sourceid);
            var subdevices = await _context.iot_device.Where(d => d.masterdeviceid == sourcedevice.id).ToListAsync();

            var newdevice = new iot_device();
            sourcedevice.CopyAllPropertiesTo(newdevice);

            newdevice.id = 0;
            newdevice.code = sourcedevice.code + "_(1)";
            newdevice.name = sourcedevice.name + " (1)";
            newdevice.masterdevice = null;
            newdevice.masterdeviceid = (masterdeviceid == 0) ? null : masterdeviceid;
            newdevice.iot_sample = null;
            newdevice.iot_subdevice = null;
            newdevice.iot_task = null;


            /*
            var lResult=await Postiot_device(iot_device);
            iot_device newdevice = (ObjectResult)lResult.Result;
            */

            _context.iot_device.Add(newdevice);
            await _context.SaveChangesAsync();
            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.INSERT, newdevice.id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            for (int i = 0; i < subdevices.Count; i++)
            {
                await Copyiot_device(subdevices[i].id, newdevice.id);
            }

            // return lResult;
            return newdevice;
        }

        public async Task<ActionResult<iot_device>> Deleteiot_device(long id)
        {
            var iot_device = await _context.iot_device.FindAsync(id);

            _context.iot_device.Remove(iot_device);
            await _context.SaveChangesAsync();

            var lMsg = new ServerUpdateHubMsg(_entity, ServerUpdateHubMsg.TOperation.DELETE, id);
            var lJson = JsonConvert.SerializeObject(lMsg);
            await _hubContext.Clients.All.SendAsync(lMsg.entity, lJson);

            return iot_device;
        }

        public bool iot_deviceExists(long id)
        {
            return _context.iot_device.Any(e => e.id == id);
        }

    }

}
