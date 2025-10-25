using System;
using QuanLyGiaoXu.Backend.Enums;
namespace QuanLyGiaoXu.Backend.DTOs.StudentDtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string StudentCode { get; set; }
        public string FullName { get; set; } // Gồm cả Tên Thánh
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } 
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? ParishDivision { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string? ClassName { get; set; } // Hiển thị tên lớp (có thể null nếu chưa xếp lớp)
    }
}