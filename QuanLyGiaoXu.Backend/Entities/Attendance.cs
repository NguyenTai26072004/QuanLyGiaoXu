using QuanLyGiaoXu.Backend.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Session { get; set; }
        public AttendanceStatus Status { get; set; }
        public string? Note { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
    }
}
