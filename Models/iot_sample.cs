using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class iot_sample
    {
        public long id { get; set; }
        public long? deviceid { get; set; }
        public long? calendardayid { get; set; }
        public DateTime timestamp { get; set; }
        public decimal? value { get; set; }
        public int? importance { get; set; }

        public virtual iot_calendarday calendarday { get; set; }
        public virtual iot_device device { get; set; }
    }
}
