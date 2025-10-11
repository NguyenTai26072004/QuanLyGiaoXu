 //Entities/Class.cs

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyGiaoXu.Backend.Entities;

/// <summary>
/// Đại diện cho một lớp học trong một năm học cụ thể.
/// </summary>
public class Class
{
    [Key] 
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } // Tên lớp, VD: "Khai Tâm 1"

    [Required]
    [MaxLength(20)]
    public string SchoolYear { get; set; } // Niên khóa, VD: "2024-2025"

    // === Foreign Key & Navigation Property cho Grade ===
    // ID của Khối mà lớp học này thuộc về.
    public int GradeId { get; set; }

    // Giúp EF Core hiểu mối quan hệ và cho phép truy cập thông tin của Khối từ Lớp học.
    public Grade Grade { get; set; }

    // === Navigation Properties ===
    // Một lớp có nhiều học sinh
    public ICollection<Student> Students { get; set; } = new List<Student>();
    // Một lớp có nhiều GLV phụ trách (thông qua bảng trung gian)
    public ICollection<UserClassAssignment> UserClassAssignments { get; set; } = new List<UserClassAssignment>();
}