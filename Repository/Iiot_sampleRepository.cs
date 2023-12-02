using System;
using System.Collections.Generic;
using currentweather.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace currentweather.Repository
{
    public interface Iiot_sampleRepository
    {
        public Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample();
        public Task<ActionResult<iot_sample>> Getiot_sample(long id);
        public Task<ActionResult<IEnumerable<iot_sample>>> Getiot_sample_devicecodedate(String code, String fromdate, String todate);
        public Task<IActionResult> Putiot_sample(long id, iot_sample iot_sample);
        public Task<int> Calcimportancetodayall();
        public Task<int> Calcimportanceall(String fromdate, String todate, int maxtochange);
        public Task<int> Calcimportanceoldest();
        public Task<int> Calcimportance(String code, String fromdate, String todate, int maxtochange);
        public Task<ActionResult<iot_sample>> Postiot_sample(iot_sample iot_sample);
        public Task<ActionResult<iot_sample>> Postiot_sample_devicecode(string pDeviceCode, iot_sample iot_sample);
        public Task<ActionResult<iot_sample>> Deleteiot_sample(long id);        
        public bool iot_sampleExists(long id);
    }
}

