using System;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.DTOs.SessionDtos;

public class CreateManualSessionDto
{
    [Required]
    public int ClassId { get; set; } // Cần biết buổi bù này cho lớp nào
    [Required]
    public DateTime SessionDate { get; set; }
    [MaxLength(200)]
    public string? Title { get; set; }
}