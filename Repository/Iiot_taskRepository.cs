using System;
using System.Collections.Generic;
using currentweather.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace currentweather.Repository
{
    public interface Iiot_taskRepository
    {
        public Task<ActionResult<IEnumerable<iot_task>>> Getiot_task();
        public Task<ActionResult<iot_task>> Getiot_task(long id);
        public Task<ActionResult<IEnumerable<iot_task>>> Getiot_task_devicecode(String code);
        public Task<IActionResult> Putiot_task(long id, iot_task iot_task);
        public Task<IActionResult> Putiot_task_completed(long id, iot_task iot_task);
        public Task<IActionResult> Putiot_task_accepted(long id, iot_task iot_task);
        public Task<ActionResult<iot_task>> Postiot_task(iot_task iot_task);
        public Task<ActionResult<iot_task>> Postiot_task_command(String code, String command);
        public Task<IActionResult> Putiot_task_failed(long id, iot_task iot_task);
        public Task<ActionResult<iot_task>> Postiot_task_devicecode(string pDeviceCode, iot_task iot_task);
        public Task<ActionResult<iot_task>> Deleteiot_task(long id);        
        public bool iot_taskExists(long id);
    }
}

