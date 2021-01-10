using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class iot_task
    {
        public long id { get; set; }
        public long deviceid { get; set; }
        public string command { get; set; }
        public string taskstatusnm { get; set; }
        public DateTime scheduled { get; set; }
        public DateTime accepted { get; set; }
        public DateTime completed { get; set; }

        public virtual iot_device device { get; set; }
    }
}
