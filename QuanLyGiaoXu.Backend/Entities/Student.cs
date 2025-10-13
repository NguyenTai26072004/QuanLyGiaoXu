using QuanLyGiaoXu.Backend.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    public string StudentCode { get; set; } // Mã thiếu nhi định danh duy nhất, VD: "TC240001"

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } // Họ tên đầy đủ, bao gồm Tên Thánh

    public DateTime DateOfBirth { get; set; }

    [Required]
    [MaxLength(10)]
    public Genders? Gender { get; set; }

    [MaxLength(150)]
    public string? FatherName { get; set; } // Họ tên cha đầy đủ, bao gồm Tên Thánh

    [MaxLength(150)]
    public string? MotherName { get; set; } // Họ tên mẹ đầy đủ, bao gồm Tên Thánh

    [MaxLength(100)]
    public string? ParishDivision { get; set; } // Giáo họ

    [MaxLength(15)]
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true; // Mặc định đang theo học

    // === Foreign Key (Khóa ngoại) ===
    // ID của lớp học mà học sinh này đang thuộc về.
    public int? ClassId { get; set; }

    // === Navigation Properties ===
    // Cho phép truy cập thông tin của lớp học từ đối tượng Student.
    public Class? Class { get; set; }

    // Một học sinh có nhiều lần điểm danh
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    // Một học sinh có nhiều điểm số
    public ICollection<Score> Scores { get; set; } = new List<Score>();
}