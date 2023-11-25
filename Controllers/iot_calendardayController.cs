using System;
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
    public class iot_calendardayController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private Iiot_calendardayRepository _iot_calendardayRepository;

        public iot_calendardayController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _iot_calendardayRepository = new iot_calendardayRepository(_context, _hubContext);

        }

        // GET: api/iot_calendarday
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_calendarday>>> Getiot_calendarday()
        {
            return await _iot_calendardayRepository.Getiot_calendarday();
        }

        // GET: api/iot_calendarday/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_calendarday>> Getiot_calendarday(long id)
        {
            if (!await _iot_calendardayRepository.Getiot_calendarday(id, out var iot_calendarday))
            {
                return NotFound();
            }

            return Ok(iot_calendarday);
        }

        // PUT: api/iot_calendarday/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<iot_calendarday>> Putiot_calendarday(long id, iot_calendarday iot_calendarday)
        {
            return await _iot_calendardayRepository.Putiot_calendarday(id, iot_calendarday);
        }

        // POST: api/iot_calendarday
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_calendarday>> Postiot_calendarday(iot_calendarday iot_calendarday)
        {
            return await _iot_calendardayRepository.Postiot_calendarday(iot_calendarday);
        }

        // POST: api/iot_calendarday/getorcreatebydate/2021-01-18
        [HttpPost("getorcreatebydate/{pDate}")]
        public async Task<ActionResult<iot_calendarday>> Getiot_calendarday_getorcreatebydate(String pDate)
        {
            return await _iot_calendardayRepository.Getiot_calendarday_getorcreatebydate(pDate);
        }


        // DELETE: api/iot_calendarday/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_calendarday>> Deleteiot_calendarday(long id)
        {
            return await _iot_calendardayRepository.Deleteiot_calendarday(id);
        }

        private bool iot_calendardayExists(long id)
        {
            return _iot_calendardayRepository.iot_calendardayExists(id);
        }
    }
}
