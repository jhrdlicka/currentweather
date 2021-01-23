using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class iot_device
    {
        public iot_device()
        {
            subdevices = new HashSet<iot_device>();
            samples = new HashSet<iot_sample>();
            tasks = new HashSet<iot_task>();
        }

        public long id { get; set; }
        public string code { get; set; }
        public string devicecategorynm { get; set; }
        public string devicetypenm { get; set; }
        public string name { get; set; }
        public string unitnm { get; set; }
        public string description { get; set; }
        public string locationnm { get; set; }
        public long? masterdeviceid { get; set; }

        public virtual iot_device masterdevice { get; set; }
        public virtual ICollection<iot_device> subdevices { get; set; }
        public virtual ICollection<iot_sample> samples { get; set; }
        public virtual ICollection<iot_task> tasks { get; set; }
    }
}
