namespace QuanLyGiaoXu.Backend.DTOs.ScheduleDtos;

public class ScheduleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan Time { get; set; }
    public string SchoolYearName { get; set; }
}