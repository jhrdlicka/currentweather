using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using currentweather.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using currentweather.Hubs;
using Microsoft.AspNetCore.SignalR;
using currentweather.Repository;

namespace currentweather.Controllers
{
    [AllowAnonymous]
    //    [Authorize(Policy = "PCMUsersOnly")]
    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class iot_deviceController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private Iiot_deviceRepository _iot_deviceRepository;

        public iot_deviceController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _iot_deviceRepository = new iot_deviceRepository(_context, _hubContext);
        }

        // GET: api/iot_device
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_device>>> Getiot_device()
        {
            return await _iot_deviceRepository.Getiot_device();
        }

        // GET: api/iot_device/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_device>> Getiot_device(long id)
        {
            return await _iot_deviceRepository.Getiot_device(id);
        }

        // PUT: api/iot_device/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<iot_device>> Putiot_device(long id, iot_device iot_device)
        {
            return await _iot_deviceRepository.Putiot_device(id, iot_device);
        }

        // POST: api/iot_device
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_device>> Postiot_device(iot_device iot_device)
        {
            return await _iot_deviceRepository.Postiot_device(iot_device);
        }


        // POST: api/iot_device/copy/5/100
        // create a new device, copy attributes from {sourceid} and reconnect it to {masterdeviceid}. It also copies subdevices of {sourceid} and attaches them under the new device
        [HttpPost("copy/{sourceid}/{masterdeviceid}")]
        public async Task<ActionResult<iot_device>> Copyiot_device(long sourceid, long? masterdeviceid)
        {
            return await _iot_deviceRepository.Copyiot_device(sourceid, masterdeviceid);
        }

        // DELETE: api/iot_device/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_device>> Deleteiot_device(long id)
        {
            return await _iot_deviceRepository.Deleteiot_device(id);
        }

        private bool iot_deviceExists(long id)
        {
            return _iot_deviceRepository.iot_deviceExists(id);
        }
    }
}
