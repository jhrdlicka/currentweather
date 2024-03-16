using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using currentweather.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using currentweather.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using currentweather.Repository;


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
        private Iiot_logRepository _iot_logRepository;

        public iot_logController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _iot_logRepository = new iot_logRepository(_context, _hubContext);

        }

        // GET: api/iot_log
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_log>>> Getiot_log()
        {
            return await _iot_logRepository.Getiot_log();
        }

        // GET: api/iot_log/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_log>> Getiot_log(long id)
        {
            if (!await _iot_logRepository.Getiot_log(id, out var iot_log))
            {
                return NotFound();
            }

            return Ok(iot_log);
        }

        // PUT: api/iot_log/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putiot_log(long id, iot_log iot_log)
        {
            return await _iot_logRepository.Putiot_log(id, iot_log);
        }

        // POST: api/iot_log
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_log>> Postiot_log(iot_log iot_log)
        {
            return await _iot_logRepository.Postiot_log(iot_log);
        }

        // POST: api/iot_log/devicecode/DEVICE01
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("devicecode/{pDeviceCode}")]
        public async Task<ActionResult<iot_log>> Postiot_log_devicecode(string pDeviceCode, iot_log iot_log)
        {
            return await _iot_logRepository.Postiot_log_devicecode(pDeviceCode, iot_log);
        }

        // DELETE: api/iot_log/older/2
        [HttpDelete("older/{days}")]
        public async Task<ActionResult<string>> Deleteolderiot_log(int days)
        {
            return await _iot_logRepository.Deleteolderiot_log(days);
        }


        // DELETE: api/iot_log/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_log>> Deleteiot_log(long id)
        {
            return await _iot_logRepository.Deleteiot_log(id);
        }

        private bool iot_logExists(long id)
        {
            return _iot_logRepository.iot_logExists(id);
        }
    }
}
