using System;
using System.Collections.Generic;
using currentweather.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace currentweather.Repository
{
    public interface Iiot_calendardayRepository
    {
        public Task<ActionResult<IEnumerable<iot_calendarday>>> Getiot_calendarday();
        public Task<bool> Getiot_calendarday(long id, out iot_calendarday iot_calendarday);
        public Task<ActionResult<iot_calendarday>> Putiot_calendarday(long id, iot_calendarday iot_calendarday);
        public Task<iot_calendarday> Postiot_calendarday(iot_calendarday iot_calendarday);
        public Task<iot_calendarday> Getiot_calendarday_getorcreatebydate(String pDate);
        public Task<ActionResult<iot_calendarday>> Deleteiot_calendarday(long id);        
        public bool iot_calendardayExists(long id);
    }
}

