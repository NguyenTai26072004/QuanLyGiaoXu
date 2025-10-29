using QuanLyGiaoXu.Backend.Enums;
using System;

namespace QuanLyGiaoXu.Backend.DTOs.AttendanceRecordDtos
{
    public class AttendanceResultDto
    {
        public string StudentCode { get; set; } 
        public string FullName { get; set; }   
        public DateTime DateOfBirth { get; set; }
        public string Status { get; set; }     
        public string? Notes { get; set; }
    }
}