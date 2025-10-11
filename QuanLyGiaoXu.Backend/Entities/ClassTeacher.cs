using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Entities
{
    public class ClassTeacher
    {
        // Composite Primary Key (sẽ được cấu hình trong DbContext)
        public required string UserId { get; set; }
        public int ClassId { get; set; }

        // **Navigation Properties**: trỏ về 2 bảng gốc
        public User User { get; set; } = null!;
        public Class Class { get; set; } = null!;
    }
}
