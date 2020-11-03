using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Identity.Models;

namespace currentweather.Models
{


    public class CurrentWeatherContext: IdentityDbContext <AppUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // For Guid Primary Key
            builder.Entity<AppUser>().Property(p => p.Id).ValueGeneratedOnAdd();

            // For int Primary Key
//            builder.Entity<AppUser>().Property(p => p.Id).UseSqlServerIdentityColumn();
        }

        public CurrentWeatherContext(DbContextOptions<CurrentWeatherContext> Options) : base(Options)
        {
        }

        public DbSet<CalendarDay> CalendarDay { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<WeatherSample> WeatherSample { get; set; }
        public DbSet<WeatherForecast> WeatherForecast { get; set; }        

    }
}
