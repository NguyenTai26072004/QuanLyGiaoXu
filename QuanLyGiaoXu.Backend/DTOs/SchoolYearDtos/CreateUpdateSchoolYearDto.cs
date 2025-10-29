using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.DTOs.SchoolYearDtos
{
    public class CreateUpdateSchoolYearDto
    {
        [Required]
        [MaxLength(20)]
        public string Year { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}