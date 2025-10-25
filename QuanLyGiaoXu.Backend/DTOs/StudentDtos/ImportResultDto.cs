using System.Collections.Generic;
namespace QuanLyGiaoXu.Backend.DTOs.StudentDtos
{
    public class ImportResultDto
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}