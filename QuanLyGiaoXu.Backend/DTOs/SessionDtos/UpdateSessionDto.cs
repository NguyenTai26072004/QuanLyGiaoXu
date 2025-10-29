using System;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.DTOs.SessionDtos;

public class UpdateSessionDto
{
    [Required]
    public DateTime SessionDate { get; set; } // Cho phép dời lịch
    [MaxLength(200)]
    public string? Title { get; set; } // Cho phép đặt chủ đề
}