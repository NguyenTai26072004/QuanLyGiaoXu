using QuanLyGiaoXu.Backend.DTOs.ClassDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services.Classes
{
    public interface IClassService
    {
        Task<List<ClassDto>> GetClassesAsync();
        Task<ClassDto?> GetClassByIdAsync(int id);
        Task<ClassDto> CreateClassAsync(CreateClassDto classDto);
        Task<bool> UpdateClassAsync(int id, UpdateClassDto classDto);
        Task<bool> DeleteClassAsync(int id);
        Task<bool> AssignTeachersToClassAsync(int classId, AssignTeacherDto assignTeacherDto);
    }
}