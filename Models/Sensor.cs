using Microsoft.AspNetCore.Builder;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Sensor
{
    public enum SensorType
    { WEATHER, 
    WEATHERFORECAST,
    WATERLEVEL,
    DISTANCE}
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Display(Name = "Sensor Type")]
    public SensorType Type { get; set; }
    public String Description { get; set; }
}