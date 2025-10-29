using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.Entities;

public class Schedule
{
    [Key]
    public int Id { get; set; }
    public int SchoolYearId { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    public int DefaultDayOfWeek { get; set; }
    public TimeSpan DefaultTime { get; set; }

    public SchoolYear SchoolYear { get; set; }
    public ICollection<Class> Classes { get; set; } = new List<Class>();
}