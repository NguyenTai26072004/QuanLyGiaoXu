using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.DTOs.ParishDivisionDtos;

public class CreateUpdateParishDivisionDto
{
    [Required(ErrorMessage = "Tên Giáo họ không được để trống.")]
    [MaxLength(100)]
    public string Name { get; set; }
}