using System.ComponentModel.DataAnnotations;

namespace QuanLyGiaoXu.Backend.DTOs.ClassDtos
{
    public class CreateClassDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int SchoolYearId { get; set; }
        [Required]
        public int GradeId { get; set; } //  Nhận vào Id của Khối
    }
}