namespace QuanLyGiaoXu.Backend.DTOs.SchoolYearDtos
{
    public class SchoolYearDto
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}