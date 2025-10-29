using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.Entities;

public class Class
{
    [Key]
    public int Id { get; set; } 
    public int? GradeId { get; set; }

    [MaxLength(100)]
    [Required] 
    public string ClassName { get; set; }

    // --- Navigation Properties ---
    public Grade? Grade { get; set; } 

    public ICollection<ClassEnrollment> Enrollments { get; set; } = new List<ClassEnrollment>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<UserClassAssignment> UserClassAssignments { get; set; } = new List<UserClassAssignment>();

    public ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();
}