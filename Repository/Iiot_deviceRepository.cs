using System;
using System.Collections.Generic;
using currentweather.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using currentweather.Hubs;
//using Microsoft.AspNetCore.SignalR;



namespace currentweather.Repository
{
    public interface Iiot_deviceRepository
    {
        public Task<ActionResult<IEnumerable<iot_device>>> Getiot_device();
        public Task<ActionResult<iot_device>> Getiot_device(long id);
        public Task<ActionResult<iot_device>> Putiot_device(long id, iot_device iot_device);
        public Task<ActionResult<iot_device>> Postiot_device(iot_device iot_device);        
        public Task<ActionResult<iot_device>> Copyiot_device(long sourceid, long? masterdeviceid);
        public Task<ActionResult<iot_device>> Deleteiot_device(long id);        
        public bool iot_deviceExists(long id);
    }
}

