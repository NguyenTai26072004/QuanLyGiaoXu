using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGiaoXu.Backend.Entities;

public class ParishDivision
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
}