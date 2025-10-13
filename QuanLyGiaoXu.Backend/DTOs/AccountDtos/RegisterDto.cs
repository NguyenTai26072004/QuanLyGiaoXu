using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.DTOs.AccountDtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}