using Microsoft.AspNetCore.Identity;
using QuanLyGiaoXu.Backend.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.Entities;
/// <summary>
/// Đại diện cho một tài khoản người dùng trong hệ thống (Admin, Huynh trưởng...).
/// Kế thừa từ IdentityUser để tận dụng hệ thống xác thực, phân quyền có sẵn của .NET.
/// Class này sẽ được ánh xạ vào bảng 'AspNetUsers' trong CSDL.
/// </summary>
public class User : IdentityUser
{
    // === Các thuộc tính tùy chỉnh mở rộng cho IdentityUser ===

    [Required] 
    [MaxLength(150)] 
    public string FullName { get; set; }

    public DateTime DateOfBirth { get; set; } 

    [MaxLength(10)]
    public Genders? Gender { get; set; } // "Nam", "Nữ"

    [MaxLength(500)]
    public string? Address { get; set; } // Địa chỉ

    [MaxLength(100)]
    public string? Position { get; set; } // Chức vụ: "Trưởng", "Phó", "Thư ký"...

    public bool IsActive { get; set; } = true; // Trạng thái hoạt động, mặc định là true

    // === Navigation Property ===
    // Giúp Entity Framework Core hiểu mối quan hệ một-nhiều: Một User có thể được phân công dạy nhiều lớp.
    // ICollection khởi tạo sẵn để tránh lỗi NullReferenceException.
    public ICollection<UserClassAssignment> UserClassAssignments { get; set; } = new List<UserClassAssignment>();

}