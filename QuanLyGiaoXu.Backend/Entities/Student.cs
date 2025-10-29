using QuanLyGiaoXu.Backend.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace QuanLyGiaoXu.Backend.Entities;

/// <summary>
/// Đại diện cho một em thiếu nhi.
/// </summary>
public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string StudentCode { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; }

    [Required] 
    public DateTime DateOfBirth { get; set; }

    [Required] 
    public Genders Gender { get; set; } 

    [MaxLength(150)]
    public string? FatherName { get; set; }

    [MaxLength(150)]
    public string? MotherName { get; set; }
    public int? ParishDivisionId { get; set; }
    public ParishDivision? ParishDivision { get; set; }

    [MaxLength(15)]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    // --- Navigation Properties ---

    // Một học sinh có thể có nhiều bản ghi xếp lớp (phục vụ cho việc chuyển lớp, lưu lịch sử)
    public ICollection<ClassEnrollment> Enrollments { get; set; } = new List<ClassEnrollment>();

    // Một học sinh có nhiều điểm số
    public ICollection<Score> Scores { get; set; } = new List<Score>();

    // Một học sinh có nhiều bản ghi kết quả điểm danh
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
}