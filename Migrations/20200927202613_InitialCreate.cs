using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace currentweather.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarDay",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Sunrise = table.Column<DateTime>(nullable: false),
                    Sunset = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecast",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalendarDay_Id = table.Column<long>(nullable: false),
                    PeriodFrom = table.Column<DateTime>(nullable: false),
                    PeriodTo = table.Column<DateTime>(nullable: false),
                    Symbol = table.Column<long>(nullable: false),
                    PrecipitationProbability = table.Column<long>(nullable: false),
                    WindDirection = table.Column<long>(nullable: false),
                    WindSpeed = table.Column<float>(nullable: false),
                    Temperature = table.Column<float>(nullable: false),
                    MinTemperature = table.Column<float>(nullable: false),
                    MaxTemperature = table.Column<float>(nullable: false),
                    Pressure = table.Column<long>(nullable: false),
                    Humidity = table.Column<long>(nullable: false),
                    Clouds = table.Column<long>(nullable: false),
                    Visibility = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecast", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherSample",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalendarDay_Id = table.Column<long>(nullable: false),
                    Sensor_Id = table.Column<long>(nullable: false),
                    SampleTime = table.Column<DateTime>(nullable: false),
                    Temperature = table.Column<float>(nullable: false),
                    Pressure = table.Column<float>(nullable: false),
                    Humidity = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherSample", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarDay");

            migrationBuilder.DropTable(
                name: "Sensor");

            migrationBuilder.DropTable(
                name: "WeatherForecast");

            migrationBuilder.DropTable(
                name: "WeatherSample");
        }
    }
}
