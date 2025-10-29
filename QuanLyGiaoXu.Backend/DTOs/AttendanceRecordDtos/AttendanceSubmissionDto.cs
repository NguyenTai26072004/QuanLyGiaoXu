using System.Collections.Generic;
namespace QuanLyGiaoXu.Backend.DTOs.AttendanceRecordDtos;

public class AttendanceRecordDto
{
    public int StudentId { get; set; }
    public Enums.AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class AttendanceSubmissionDto
{
    public List<AttendanceRecordDto> Records { get; set; } = new();
}