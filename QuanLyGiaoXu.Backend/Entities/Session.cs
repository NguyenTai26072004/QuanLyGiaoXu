using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.Entities;

public class Session
{
    [Key]
    public int Id { get; set; }
    public int ClassId { get; set; }
    public DateTime SessionDate { get; set; }
    [MaxLength(200)]
    public string? Title { get; set; }
    public bool IsCancelled { get; set; } = false;
    public int SessionType { get; set; } = 1;
    public DateTime? OriginalSessionDate { get; set; }

    public Class Class { get; set; }
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
}