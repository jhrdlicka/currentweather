using Microsoft.AspNetCore.Builder;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CalendarDay
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    [Required] 
    public DateTime Date { get; set; }
    public DateTime Sunrise { get; set; }
    public DateTime Sunset { get; set; }
}