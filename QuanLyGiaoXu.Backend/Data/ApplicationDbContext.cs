using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Entities;

namespace QuanLyGiaoXu.Backend.Data
{
    // Sử dụng khai báo tường minh để đảm bảo tính nhất quán
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // === DANH SÁCH CÁC BẢNG (DbSet) THEO MÔ HÌNH MỚI NHẤT ===
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ParishDivision> ParishDivisions { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Score> Scores { get; set; }

        // --- Các bảng trung gian ---
        public DbSet<UserClassAssignment> UserClassAssignments { get; set; } 
        public DbSet<ClassEnrollment> ClassEnrollments { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- CẤU HÌNH CÁC RÀNG BUỘC VÀ KHÓA ---

            // Cấu hình Unique Constraints
            builder.Entity<SchoolYear>().HasIndex(sy => sy.Year).IsUnique();
            builder.Entity<Grade>().HasIndex(g => g.Name).IsUnique();
            builder.Entity<Student>().HasIndex(s => s.StudentCode).IsUnique();

            // Cấu hình các bảng trung gian có khóa chính kết hợp (Composite Key)

            // 1. UserClassAssignment (GLV <-> Lớp)
            builder.Entity<UserClassAssignment>().HasKey(uca => new { uca.UserId, uca.ClassId });

            // 2. AttendanceRecord (Học sinh <-> Buổi học)
            builder.Entity<AttendanceRecord>().HasKey(ar => new { ar.StudentId, ar.SessionId }); 

            // 3. (MỚI) ClassSchedule (Lớp <-> Khuôn mẫu Lịch)
            builder.Entity<ClassSchedule>().HasKey(cs => new { cs.ClassId, cs.ScheduleId });


            // --- CẤU HÌNH CÁC MỐI QUAN HỆ (Relationships) TƯỜNG MINH ---
            // Cấu hình quan hệ cho ClassSchedule
            builder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.ClassSchedules)
                .HasForeignKey(cs => cs.ClassId);

            builder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Schedule)
                .WithMany() 
                .HasForeignKey(cs => cs.ScheduleId);

            builder.Entity<ParishDivision>()
                .HasIndex(pd => pd.Name)
                .IsUnique();

            builder.Entity<UserClassAssignment>()
                .HasOne(uca => uca.User)
                .WithMany(u => u.UserClassAssignments)
                .HasForeignKey(uca => uca.UserId);

            builder.Entity<UserClassAssignment>()
                .HasOne(uca => uca.Class)
                .WithMany(c => c.UserClassAssignments)
                .HasForeignKey(uca => uca.ClassId);

            builder.Entity<ClassEnrollment>()
                .HasOne(ce => ce.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(ce => ce.StudentId);

            builder.Entity<ClassEnrollment>()
                .HasOne(ce => ce.Class)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(ce => ce.ClassId);

            builder.Entity<AttendanceRecord>()
                .HasOne(ar => ar.Student)
                .WithMany(s => s.AttendanceRecords)
                .HasForeignKey(ar => ar.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}