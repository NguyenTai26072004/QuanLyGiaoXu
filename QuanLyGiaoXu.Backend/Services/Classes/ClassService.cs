using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.ClassDtos;
using QuanLyGiaoXu.Backend.Entities;
using QuanLyGiaoXu.Backend.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
{
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ClassService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // --- SỬA LẠI: DÙNG ProjectTo cho hiệu quả ---
        public async Task<List<ClassDto>> GetClassesAsync()
        {
            return await _context.Classes
                .ProjectTo<ClassDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // --- SỬA LẠI: DÙNG ProjectTo cho hiệu quả ---
        public async Task<ClassDto?> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Where(c => c.Id == id)
                .ProjectTo<ClassDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        // --- SỬA LỖI LOGIC TRIỆT ĐỂ Ở ĐÂY ---
        public async Task<ClassDto> CreateClassAsync(CreateClassDto classDto)
        {
            // Bước 1: Mapping DTO sang Entity
            var newClass = _mapper.Map<Class>(classDto);

            // Bước 2: Lưu vào CSDL
            await _context.Classes.AddAsync(newClass);
            await _context.SaveChangesAsync();

            // Bước 3: **RẤT QUAN TRỌNG** - Lấy lại thông tin đầy đủ của Class vừa tạo từ CSDL
            // Bằng cách gọi lại GetClassByIdAsync, chúng ta đảm bảo tất cả dữ liệu liên quan được tải đúng
            // và ProjectTo sẽ hoạt động chính xác.
            var createdClassDto = await GetClassByIdAsync(newClass.Id);

            // Nếu không tìm thấy vì một lý do nào đó, throw lỗi.
            if (createdClassDto == null)
            {
                throw new Exception("Không thể tạo hoặc lấy lại thông tin lớp học.");
            }

            return createdClassDto;
        }

        public async Task<bool> UpdateClassAsync(int id, UpdateClassDto classDto)
        {
            var existingClass = await _context.Classes.FindAsync(id);
            if (existingClass == null) return false;

            _mapper.Map(classDto, existingClass);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var existingClass = await _context.Classes.FindAsync(id);
            if (existingClass == null) return false;

            if (await _context.Students.AnyAsync(s => s.ClassId == id))
            {
                throw new InvalidOperationException("Không thể xóa lớp này vì đang có học sinh.");
            }

            _context.UserClassAssignments.RemoveRange(existingClass.UserClassAssignments); // Xóa các phân công liên quan
            _context.Classes.Remove(existingClass);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignTeachersToClassAsync(int classId, AssignTeacherDto assignTeacherDto)
        {
            var existingClass = await _context.Classes
                .Include(c => c.UserClassAssignments)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (existingClass == null) return false;

            // Xóa các phân công cũ
            _context.UserClassAssignments.RemoveRange(existingClass.UserClassAssignments);

            // Tạo các phân công mới
            foreach (var teacherId in assignTeacherDto.TeacherIds)
            {
                _context.UserClassAssignments.Add(new UserClassAssignment { ClassId = classId, UserId = teacherId });
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}