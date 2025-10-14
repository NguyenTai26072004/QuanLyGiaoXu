using System.Collections.Generic;

namespace QuanLyGiaoXu.Backend.DTOs.ClassDtos
{
    // DTO này hiển thị thông tin chi tiết của một Lớp học
    public class ClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SchoolYearName { get; set; }
        public string GradeName { get; set; } // Hiển thị tên Khối
        public List<string> TeacherNames { get; set; } = new List<string>(); // Danh sách tên các GLV
        public int NumberOfStudents { get; set; } // Sĩ số lớp
    }
}