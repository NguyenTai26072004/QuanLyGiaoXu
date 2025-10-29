using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.EnrollmentDtos;
using QuanLyGiaoXu.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.Enrollments;

public class EnrollmentService : IEnrollmentService
{
    private readonly ApplicationDbContext _context;

    public EnrollmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task EnrollStudentsAsync(int classId, EnrollStudentsDto enrollDto)
    {
        // Kiểm tra xem lớp học có tồn tại không
        var classExists = await _context.Classes.AnyAsync(c => c.Id == classId);
        if (!classExists)
        {
            throw new KeyNotFoundException("Không tìm thấy lớp học.");
        }

        var enrollmentsToCreate = new List<ClassEnrollment>();
        var now = DateTime.UtcNow;

        // Lấy danh sách các học sinh đã được xếp vào lớp này rồi
        var existingEnrollments = await _context.ClassEnrollments
            .Where(e => e.ClassId == classId && e.EndDate == null) // Chỉ quan tâm các em đang học
            .Select(e => e.StudentId)
            .ToListAsync();

        foreach (var studentId in enrollDto.StudentIds)
        {
            // Chỉ thêm vào nếu học sinh chưa được xếp vào lớp này
            if (!existingEnrollments.Contains(studentId))
            {
                enrollmentsToCreate.Add(new ClassEnrollment
                {
                    ClassId = classId,
                    StudentId = studentId,
                    EnrollDate = now
                });
            }
        }

        if (enrollmentsToCreate.Any())
        {
            await _context.ClassEnrollments.AddRangeAsync(enrollmentsToCreate);
            await _context.SaveChangesAsync();
        }
    }
}