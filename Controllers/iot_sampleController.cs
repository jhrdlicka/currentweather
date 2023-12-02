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
    public class iot_sampleController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private Iiot_sampleRepository _iot_sampleRepository;

        public iot_sampleController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _iot_sampleRepository = new iot_sampleRepository(_context, _hubContext);
        }

        // GET: api/iot_sample
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample()
        {
            return await _iot_sampleRepository.Getiot_sample();
        }

        // GET: api/iot_sample/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_sample>> Getiot_sample(long id)
        {
            return await _iot_sampleRepository.Getiot_sample(id);
        }

        // GET: api/iot_sample/devicecodedate/DEVICE01/2021-01-17/2199-12-31
        [HttpGet("devicecodedate/{code}/{fromdate}/{todate}")]
        public async Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample_devicecodedate(String code, String fromdate, String todate)
        {
            return await _iot_sampleRepository.Getiot_sample_devicecodedate(code, fromdate, todate);
        }

        // PUT: api/iot_sample/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putiot_sample(long id, iot_sample iot_sample)
        {
            return await _iot_sampleRepository.Putiot_sample(id, iot_sample);
        }

        // PUT: api/iot_sample/calcimportancetodayall
        [HttpPut("Calcimportancetodayall")]
        public async Task<IActionResult> Calcimportancetodayall()
        {
            return Ok(await _iot_sampleRepository.Calcimportancetodayall());
        }

        // PUT: api/iot_sample/calcimportanceall/2021-01-17/2199-12-31
        [HttpPut("calcimportanceall/{fromdate}/{todate}")]
        public async Task<IActionResult> Calcimportanceall(String fromdate, String todate)
        {
            return Ok(await _iot_sampleRepository.Calcimportanceall(fromdate, todate, 999999));
        }

        // PUT: api/iot_sample/calcimportanceoldest
        [HttpPut("calcimportanceoldest")]
        public async Task<IActionResult> Calcimportanceoldest()
        {
            //var lChanged = await _iot_sampleRepository.Calcimportanceoldest();
            return Ok(await _iot_sampleRepository.Calcimportanceoldest());
        }

        // PUT: api/iot_sample/calcimportance/DEVICE01/2021-01-17/2199-12-31
        [HttpPut("calcimportance/{code}/{fromdate}/{todate}")]
        public async Task<IActionResult> Calcimportance(String code, String fromdate, String todate)
        {
            return Ok(await _iot_sampleRepository.Calcimportance(code, fromdate, todate, 999999));
        }


        // POST: api/iot_sample
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<iot_sample>> Postiot_sample(iot_sample iot_sample)
        {
            return await _iot_sampleRepository.Postiot_sample(iot_sample);
        }


        // POST: api/iot_sample/devicecode/DEVICE01
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("devicecode/{pDeviceCode}")]
        public async Task<ActionResult<iot_sample>> Postiot_sample_devicecode(string pDeviceCode, iot_sample iot_sample)
        {
            return await _iot_sampleRepository.Postiot_sample_devicecode(pDeviceCode, iot_sample);
        }


        // DELETE: api/iot_sample/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_sample>> Deleteiot_sample(long id)
        {
            return await _iot_sampleRepository.Deleteiot_sample(id);
        }

        private bool iot_sampleExists(long id)
        {
            return _iot_sampleRepository.iot_sampleExists(id);
        }
    }
}
