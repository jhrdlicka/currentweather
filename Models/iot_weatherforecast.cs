using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class iot_weatherforecast
    {
        public long id { get; set; }
        public long? calendardayid { get; set; }
        public DateTime periodfrom { get; set; }
        public DateTime periodto { get; set; }
        public int? symbol { get; set; }
        public long? precipitationprobability { get; set; }
        public long? winddirection { get; set; }
        public float? windspeed { get; set; }
        public float? temperature { get; set; }
        public float? mintemperature { get; set; }
        public float? maxtemperature { get; set; }
        public long? pressure { get; set; }
        public long? humidity { get; set; }
        public long? clouds { get; set; }
        public long? visibility { get; set; }
        public string locationnm { get; set; }
        public DateTime issued { get; set; }
        public bool valid { get; set; }

        public virtual iot_calendarday calendarday { get; set; }
    }
}
