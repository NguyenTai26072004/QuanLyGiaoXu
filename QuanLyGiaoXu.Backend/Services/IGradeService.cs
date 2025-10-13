using QuanLyGiaoXu.Backend.DTOs.GradeDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
{
    public interface IGradeService
    {
        Task<List<GradeDto>> GetGradesAsync();
        Task<GradeDto> GetGradeByIdAsync(int id);
        Task<GradeDto> CreateGradeAsync(CreateUpdateGradeDto createGradeDto);
        Task<bool> UpdateGradeAsync(int id, CreateUpdateGradeDto updateGradeDto);
        Task<bool> DeleteGradeAsync(int id);
    }
}