using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.ClassDtos;
using QuanLyGiaoXu.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services.Classes
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

        public async Task<List<ClassDetailDto>> GetClassesAsync()
        {
            return await _context.Classes
                .OrderBy(c => c.ClassName)
                .ProjectTo<ClassDetailDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ClassDetailDto?> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Where(c => c.Id == id)
                .ProjectTo<ClassDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<ClassDetailDto> CreateClassAndGenerateSessionsAsync(CreateClassDto createClassDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // === Bước 1: Tạo Lớp học cơ bản ===
                var newClass = new Class
                {
                    ClassName = createClassDto.ClassName,
                    GradeId = createClassDto.GradeId
                };
                await _context.Classes.AddAsync(newClass);
                await _context.SaveChangesAsync();

                // === Bước 2: Tạo Lịch trình và Sinh Sessions ===
                var sessionsToCreate = new List<Session>();

                // Lặp qua từng ScheduleId mà người dùng gửi lên
                foreach (var scheduleId in createClassDto.ScheduleIds)
                {
                    // Thêm bản ghi vào bảng trung gian ClassSchedules
                    _context.ClassSchedules.Add(new ClassSchedule { ClassId = newClass.Id, ScheduleId = scheduleId });

                    // Lấy thông tin khuôn mẫu lịch
                    var schedule = await _context.Schedules
                        .Include(s => s.SchoolYear)
                        .FirstOrDefaultAsync(s => s.Id == scheduleId);

                    if (schedule == null || schedule.SchoolYear == null)
                        throw new KeyNotFoundException($"Khuôn mẫu lịch học với ID {scheduleId} không hợp lệ.");

                    // Sinh Sessions cho khuôn mẫu lịch này
                    var startDate = schedule.SchoolYear.StartDate;
                    var endDate = schedule.SchoolYear.EndDate;
                    var dayOfWeek = (DayOfWeek)schedule.DefaultDayOfWeek;
                    var timeOfDay = schedule.DefaultTime;

                    for (var currentDate = startDate.Date; currentDate <= endDate.Date; currentDate = currentDate.AddDays(1))
                    {
                        if (currentDate.DayOfWeek == dayOfWeek)
                        {
                            sessionsToCreate.Add(new Session
                            {
                                ClassId = newClass.Id,
                                SessionDate = currentDate.Date + timeOfDay,
                                SessionType = 1,
                            });
                        }
                    }
                }

                if (sessionsToCreate.Any())
                {
                    await _context.Sessions.AddRangeAsync(sessionsToCreate);
                }

                // Lưu tất cả các thay đổi (ClassSchedules và Sessions)
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetClassByIdAsync(newClass.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateClassAsync(int id, UpdateClassDto updateClassDto)
        {
            var existingClass = await _context.Classes.FindAsync(id);
            if (existingClass == null) return false;

            _mapper.Map(updateClassDto, existingClass);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var existingClass = await _context.Classes.FindAsync(id);
            if (existingClass == null) return false;

            if (await _context.ClassEnrollments.AnyAsync(e => e.ClassId == id))
            {
                throw new InvalidOperationException("Không thể xóa lớp này vì đang có học sinh được xếp lớp.");
            }

            // Cũng nên xóa các Sessions liên quan
            var sessions = _context.Sessions.Where(s => s.ClassId == id);
            _context.Sessions.RemoveRange(sessions);

            // Xóa các phân công liên quan
            var assignments = _context.UserClassAssignments.Where(a => a.ClassId == id);
            _context.UserClassAssignments.RemoveRange(assignments);

            _context.Classes.Remove(existingClass);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AssignTeachersAsync(int id, AssignTeachersDto assignTeachersDto)
        {
            var existingClass = await _context.Classes.Include(c => c.UserClassAssignments)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (existingClass == null) return false;

            // Xóa các phân công cũ
            _context.UserClassAssignments.RemoveRange(existingClass.UserClassAssignments);

            // Thêm các phân công mới
            foreach (var teacherId in assignTeachersDto.TeacherIds)
            {
                _context.UserClassAssignments.Add(new UserClassAssignment { ClassId = id, UserId = teacherId });
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}