using QuanLyGiaoXu.Backend.DTOs.SchoolYearDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
{
    public interface ISchoolYearService
    {
        Task<List<SchoolYearDto>> GetSchoolYearsAsync();
        Task<SchoolYearDto> CreateSchoolYearAsync(CreateUpdateSchoolYearDto schoolYearDto);
        Task<bool> UpdateSchoolYearAsync(int id, CreateUpdateSchoolYearDto schoolYearDto);
    }
}