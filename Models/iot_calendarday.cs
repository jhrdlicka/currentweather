using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class iot_calendarday
    {
        public iot_calendarday()
        {
            iot_sample = new HashSet<iot_sample>();
            iot_weatherforecast = new HashSet<iot_weatherforecast>();
        }

        public long id { get; set; }
        public DateTime date { get; set; }
        public DateTime? sunrise { get; set; }
        public DateTime? sunset { get; set; }

        public virtual ICollection<iot_sample> iot_sample { get; set; }
        public virtual ICollection<iot_weatherforecast> iot_weatherforecast { get; set; }
    }
}
