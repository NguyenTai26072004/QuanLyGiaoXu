using QuanLyGiaoXu.Backend.Enums;
namespace QuanLyGiaoXu.Backend.Entities;

public class AttendanceRecord
{
    public int StudentId { get; set; }
    public int SessionId { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }

    public Student Student { get; set; }
    public Session Session { get; set; }
}