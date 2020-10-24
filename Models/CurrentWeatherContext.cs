using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace currentweather.Models
{
    public class CurrentWeatherContext: DbContext
    {
        public CurrentWeatherContext(DbContextOptions<CurrentWeatherContext> options)
            : base(options)
        {
        }

        public DbSet<CalendarDay> CalendarDay { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<WeatherSample> WeatherSample { get; set; }
        public DbSet<WeatherForecast> WeatherForecast { get; set; }

    }
}
