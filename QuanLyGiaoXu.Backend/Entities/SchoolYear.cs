using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyGiaoXu.Backend.Entities;

public class SchoolYear
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Year { get; set; } // Ví dụ: "2025-2026"
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }   

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

}