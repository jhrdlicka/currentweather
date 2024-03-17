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
    public class iot_taskController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;
        private readonly IHubContext<ServerUpdateHub> _hubContext;
        private Iiot_taskRepository _iot_taskRepository;

        public iot_taskController(CurrentWeatherContext context, IHubContext<ServerUpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _iot_taskRepository = new iot_taskRepository(_context, _hubContext);
        }

        // GET: api/iot_task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iot_task>>> Getiot_task()
        {
            return await _iot_taskRepository.Getiot_task();
        }

        // GET: api/iot_task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iot_task>> Getiot_task(long id)
        {
            return await _iot_taskRepository.Getiot_task(id);
        }

        // GET: api/iot_task/devicecode/DEVICE01
        [HttpGet("devicecode/{code}")]
        public async Task<ActionResult<IEnumerable<iot_task>>> Getiot_task_devicecode(String code)
        {
            return await _iot_taskRepository.Getiot_task_devicecode(code);
        }


        // PUT: api/iot_task/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putiot_task(long id, iot_task iot_task)
        {
            return await _iot_taskRepository.Putiot_task(id, iot_task);
        }


        // PUT: api/iot_task/completed/5
        [HttpPut("completed/{id}")]
        public async Task<IActionResult> Putiot_task_completed(long id, iot_task iot_task)
        {
            return await _iot_taskRepository.Putiot_task_completed(id, iot_task);
        }

        // PUT: api/iot_task/accepted/5
        [HttpPut("accepted/{id}")]
        public async Task<IActionResult> Putiot_task_accepted(long id, iot_task iot_task)
        {
            return await _iot_taskRepository.Putiot_task_accepted(id, iot_task);
        }

        // PUT: api/iot_task/failed/5
        [HttpPut("failed/{id}")]
        public async Task<IActionResult> Putiot_task_failed(long id, iot_task iot_task)
        {
            return await _iot_taskRepository.Putiot_task_failed(id, iot_task);
        }

        // POST: api/iot_task
        [HttpPost]
        public async Task<ActionResult<iot_task>> Postiot_task(iot_task iot_task)
        {
            return await _iot_taskRepository.Postiot_task(iot_task);
        }

        // POST: api/iot_task/switchon/DEVICE01
        [HttpPost("switchon/{code}")]
        public async Task<ActionResult<iot_task>> Postiot_task_switchon(String code)
        {
            return await _iot_taskRepository.Postiot_task_command(code, "SWITCH ON");
        }

        // POST: api/iot_task/switchoff/DEVICE01
        [HttpPost("switchoff/{code}")]
        public async Task<ActionResult<iot_task>> Postiot_task_switchoff(String code)
        {
            return await _iot_taskRepository.Postiot_task_command(code, "SWITCH OFF");
        }

        // POST: api/iot_task/ping/DEVICE01
        [HttpPost("ping/{code}")]
        public async Task<ActionResult<iot_task>> Postiot_task_ping(String code)
        {
            return await _iot_taskRepository.Postiot_task_command(code, "PING");
        }

        // POST: api/iot_task/upgradefirmware/DEVICE01
        [HttpPost("upgradefirmware/{code}")]
        public async Task<ActionResult<iot_task>> Postiot_task_upgradefirmware(String code)
        {
            return await _iot_taskRepository.Postiot_task_command(code, "UPGRADE FIRMWARE");
        }

        // DELETE: api/iot_task/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<iot_task>> Deleteiot_task(long id)
        {
            return await _iot_taskRepository.Deleteiot_task(id);
        }
    }
}
