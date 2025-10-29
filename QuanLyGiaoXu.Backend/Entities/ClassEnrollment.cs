using System;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.Entities;

public class ClassEnrollment
{
    [Key]
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateTime EnrollDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Student Student { get; set; }
    public Class Class { get; set; }
}