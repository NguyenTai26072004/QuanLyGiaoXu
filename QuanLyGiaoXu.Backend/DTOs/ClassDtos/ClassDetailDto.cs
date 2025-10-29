using System.Collections.Generic;
namespace QuanLyGiaoXu.Backend.DTOs.ClassDtos;

public class ClassDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? GradeName { get; set; }
    public string ScheduleName { get; set; }
    public List<string> TeacherNames { get; set; } = new();
    public int NumberOfStudents { get; set; }
}