namespace QuanLyGiaoXu.Backend.Entities;

public class ClassSchedule
{
    public int ClassId { get; set; }   // Khóa ngoại đến Class
    public int ScheduleId { get; set; } // Khóa ngoại đến Schedule

    // Navigation Properties
    public Class Class { get; set; }
    public Schedule Schedule { get; set; }
}