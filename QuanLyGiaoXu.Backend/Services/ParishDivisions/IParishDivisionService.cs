using QuanLyGiaoXu.Backend.DTOs.ParishDivisionDtos;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.ParishDivisions;

public interface IParishDivisionService
{
    Task<List<ParishDivisionDto>> GetParishDivisionsAsync();
    Task<ParishDivisionDto> CreateParishDivisionAsync(CreateUpdateParishDivisionDto dto);
    Task<bool> UpdateParishDivisionAsync(int id, CreateUpdateParishDivisionDto dto);
    Task<bool> DeleteParishDivisionAsync(int id);
}