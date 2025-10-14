using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace QuanLyGiaoXu.Backend.Data 
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<UserClassAssignment> UserClassAssignments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserClassAssignment>().HasKey(uca => new { uca.UserId, uca.ClassId });

            builder.Entity<UserClassAssignment>().HasOne(uca => uca.User)
                .WithMany(u => u.UserClassAssignments).HasForeignKey(uca => uca.UserId);

            builder.Entity<UserClassAssignment>().HasOne(uca => uca.Class)
                .WithMany(c => c.UserClassAssignments).HasForeignKey(uca => uca.ClassId);

            builder.Entity<Student>().HasIndex(s => s.StudentCode).IsUnique();

            builder.Entity<Grade>()
                .HasIndex(g => g.Name)
                .IsUnique();

            builder.Entity<SchoolYear>()
                .HasIndex(sy => sy.Year)
                .IsUnique();


        }
    }
}

