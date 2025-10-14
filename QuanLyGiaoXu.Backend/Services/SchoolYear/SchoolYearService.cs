using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.SchoolYearDtos;
using QuanLyGiaoXu.Backend.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
{
    public class SchoolYearService : ISchoolYearService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SchoolYearService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SchoolYearDto>> GetSchoolYearsAsync()
        {
            return await _context.SchoolYears
                .OrderByDescending(sy => sy.Year) // Sắp xếp năm học mới nhất lên đầu
                .ProjectTo<SchoolYearDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<SchoolYearDto> CreateSchoolYearAsync(CreateUpdateSchoolYearDto schoolYearDto)
        {
            var schoolYear = _mapper.Map<SchoolYear>(schoolYearDto);
            await _context.SchoolYears.AddAsync(schoolYear);
            await _context.SaveChangesAsync();
            return _mapper.Map<SchoolYearDto>(schoolYear);
        }

        public async Task<bool> UpdateSchoolYearAsync(int id, CreateUpdateSchoolYearDto schoolYearDto)
        {
            var schoolYear = await _context.SchoolYears.FindAsync(id);
            if (schoolYear == null) return false;

            // Dùng AutoMapper để cập nhật từ DTO
            _mapper.Map(schoolYearDto, schoolYear);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}