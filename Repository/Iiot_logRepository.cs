using System;
using System.Collections.Generic;
using currentweather.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace currentweather.Repository
{
    public interface Iiot_logRepository
    {
        public Task<ActionResult<IEnumerable<iot_log>>> Getiot_log();
        public Task<bool> Getiot_log(long id, out iot_log iot_log);
        public Task<IActionResult> Putiot_log(long id, iot_log iot_log);
        public Task<ActionResult<iot_log>> Postiot_log(iot_log iot_log);
        public Task<ActionResult<iot_log>> Postiot_log_devicecode(string pDeviceCode, iot_log iot_log);
        public Task<string> Deleteolderiot_log(int days);
        public Task<ActionResult<iot_log>> Deleteiot_log(long id);        
        public bool iot_logExists(long id);
    }
}
