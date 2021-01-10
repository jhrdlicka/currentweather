using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class iot_device
    {
        public iot_device()
        {
            iot_sample = new HashSet<iot_sample>();
            iot_task = new HashSet<iot_task>();
        }

        public long id { get; set; }
        public string code { get; set; }
        public string devicecategorynm { get; set; }
        public string devicetypenm { get; set; }
        public string name { get; set; }
        public string unitnm { get; set; }
        public string description { get; set; }
        public string locationnm { get; set; }

        public virtual ICollection<iot_sample> iot_sample { get; set; }
        public virtual ICollection<iot_task> iot_task { get; set; }
    }
}
