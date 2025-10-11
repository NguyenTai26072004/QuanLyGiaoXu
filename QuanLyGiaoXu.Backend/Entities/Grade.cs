using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyGiaoXu.Backend.Entities;

/// <summary>
/// Đại diện cho một Khối hoặc Ngành học trong giáo xứ (VD: Ấu Nhi, Thiếu Nhi...).
/// Tách ra bảng riêng giúp Admin có thể quản lý danh sách này một cách linh hoạt.
/// </summary>
public class Grade
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    // === Navigation Property ===
    // Một Khối có thể có nhiều Lớp học thuộc về nó.
    public ICollection<Class> Classes { get; set; } = new List<Class>();
}