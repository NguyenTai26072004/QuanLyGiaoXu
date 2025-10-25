using QuanLyGiaoXu.Backend.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace QuanLyGiaoXu.Backend.DTOs.StudentDtos
{
    public class UpdateStudentDto
    {      

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Genders Gender { get; set; }

        [MaxLength(150)]
        public string? FatherName { get; set; }

        [MaxLength(150)]
        public string? MotherName { get; set; }

        [MaxLength(100)]
        public string? ParishDivision { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public int? ClassId { get; set; }
    }
}