using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.DTOs.EnrollmentDtos;

// DTO này nhận vào một danh sách các StudentId để xếp vào lớp
public class EnrollStudentsDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Phải chọn ít nhất một học sinh.")]
    public List<int> StudentIds { get; set; }
}