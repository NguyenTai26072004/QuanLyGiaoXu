using System.ComponentModel.DataAnnotations;

namespace QuanLyGiaoXu.Backend.DTOs.ClassDtos
{
    public class UpdateClassDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string SchoolYear { get; set; }
        [Required]
        public int GradeId { get; set; }
    }
}