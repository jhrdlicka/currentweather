using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace currentweather.Models
{
    public class WeatherSample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("CalendarDay")]
        public long CalendarDay_Id { get; set; }
        [ForeignKey("Sensor")]
        public long Sensor_Id { get; set; }
        public DateTime SampleTime { get; set; }
        public float Temperature { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
    }
}