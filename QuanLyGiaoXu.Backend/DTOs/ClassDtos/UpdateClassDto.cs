using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.DTOs.ClassDtos;

public class UpdateClassDto
{
    [Required] public string ClassName { get; set; }
    public int? GradeId { get; set; }
    [Required, MinLength(1, ErrorMessage = "Phải chọn ít nhất một lịch trình.")]
    public List<int> ScheduleIds { get; set; } = new();
}