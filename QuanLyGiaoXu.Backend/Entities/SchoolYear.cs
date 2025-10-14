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

    public ICollection<Class> Classes { get; set; } = new List<Class>();
}