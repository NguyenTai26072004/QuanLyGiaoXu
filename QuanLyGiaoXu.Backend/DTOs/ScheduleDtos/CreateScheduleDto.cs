using System;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.DTOs.ScheduleDtos;

public class CreateScheduleDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public int SchoolYearId { get; set; }
    [Range(0, 6)] // 0=Sunday, 6=Saturday
    public int DefaultDayOfWeek { get; set; }
    [Required]
    public TimeSpan DefaultTime { get; set; }
}