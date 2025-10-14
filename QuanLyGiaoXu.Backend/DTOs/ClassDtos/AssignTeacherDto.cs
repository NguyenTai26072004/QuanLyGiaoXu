using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyGiaoXu.Backend.DTOs.ClassDtos
{
    // DTO này dùng để nhận danh sách ID của các GLV cần gán vào một lớp
    public class AssignTeacherDto
    {
        [Required]
        // Danh sách các UserId của GLV
        public List<string> TeacherIds { get; set; }
    }
}