using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.GradeDtos;
using QuanLyGiaoXu.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
{
    public class GradeService : IGradeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GradeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GradeDto>> GetGradesAsync()
        {
            return await _context.Grades
                .OrderBy(g => g.Name)
                .ProjectTo<GradeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<GradeDto?> GetGradeByIdAsync(int id)
        {
            return await _context.Grades
                .ProjectTo<GradeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<GradeDto> CreateGradeAsync(CreateUpdateGradeDto createGradeDto)
        {
            var grade = _mapper.Map<Grade>(createGradeDto);
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return _mapper.Map<GradeDto>(grade);
        }

        public async Task<bool> UpdateGradeAsync(int id, CreateUpdateGradeDto updateGradeDto)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return false;

            _mapper.Map(updateGradeDto, grade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGradeAsync(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return false;

            var isUsed = await _context.Classes.AnyAsync(c => c.GradeId == id);
            if (isUsed)
            {
                throw new InvalidOperationException("Không thể xóa Khối này vì đang có Lớp học thuộc về nó.");
            }

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}