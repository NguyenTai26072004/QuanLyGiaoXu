using QuanLyGiaoXu.Backend.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Entities
{
    public class Score
    {
        public int Id { get; set; }
        public double ScoreValue { get; set; }
        public ScoreType ScoreType { get; set; }
        public DateTime Date { get; set; }
        public Semester Semester { get; set; }
        public string SchoolYear { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
    }
}
