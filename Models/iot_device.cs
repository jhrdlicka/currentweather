using System;
using System.Collections.Generic;

namespace currentweather.Models
{
    public partial class iot_device
    {
        public iot_device()
        {
            iot_subdevice = new HashSet<iot_device>();
            iot_sample = new HashSet<iot_sample>();
            iot_task = new HashSet<iot_task>();
            iot_log = new HashSet<iot_log>();
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
        public virtual ICollection<iot_device> iot_subdevice { get; set; }
        public virtual ICollection<iot_sample> iot_sample { get; set; }
        public virtual ICollection<iot_task> iot_task { get; set; }
        public virtual ICollection<iot_log> iot_log { get; set; }

        public void CopyAllPropertiesTo<T>(T target)
        {
            var source = this;
            var type = typeof(T);

            foreach (var sourceProperty in type.GetProperties())
            {
                var targetProperty = type.GetProperty(sourceProperty.Name);
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
            foreach (var sourceField in type.GetFields())
            {
                var targetField = type.GetField(sourceField.Name);
                targetField.SetValue(target, sourceField.GetValue(source));
            }
        }
    }
}
