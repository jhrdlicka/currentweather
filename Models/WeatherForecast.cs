using Microsoft.AspNetCore.Builder;
using System;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class WeatherForecast
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [ForeignKey("CalendarDay")]
    public long CalendarDay_Id { get; set; }
    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }
    public long Symbol { get; set; }
public long PrecipitationProbability { get; set; }
public long WindDirection { get; set; }
public float WindSpeed { get; set; }
public float Temperature { get; set; }
public float MinTemperature { get; set; }
public float MaxTemperature { get; set; }
    public long Pressure { get; set; }
    public long Humidity { get; set; }
public long Clouds { get; set; }
public long Visibility { get; set; }


/*
< symbol number = "804" name = "overcast clouds" var = "04d" />
     < precipitation probability = "0" />
      < windDirection deg = "207" code = "SSW" name = "South-southwest" />
           < windSpeed mps = "1.98" unit = "m/s" name = "Light breeze" />
                < temperature unit = "kelvin" value = "285.59" min = "285.59" max = "285.62" />
                       < feels_like value = "283.06" unit = "kelvin" />
                          < pressure unit = "hPa" value = "1005" />
                             < humidity value = "60" unit = "%" />
                                < clouds value = "overcast clouds" all = "85" unit = "%" />
                                     < visibility value = "10000" />
*/
}