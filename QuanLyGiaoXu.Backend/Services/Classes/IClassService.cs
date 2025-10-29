using QuanLyGiaoXu.Backend.DTOs.ClassDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services.Classes;

public interface IClassService
{
    // Lấy danh sách tóm tắt tất cả các lớp
    Task<List<ClassDetailDto>> GetClassesAsync();
    // Lấy thông tin chi tiết của 1 lớp
    Task<ClassDetailDto?> GetClassByIdAsync(int id);
    // Phương thức "kỳ diệu": Tạo lớp VÀ tự động sinh Sessions
    Task<ClassDetailDto> CreateClassAndGenerateSessionsAsync(CreateClassDto createClassDto);
    // Cập nhật thông tin cơ bản của lớp
    Task<bool> UpdateClassAsync(int id, UpdateClassDto updateClassDto);
    // Xóa một lớp (kèm các kiểm tra)
    Task<bool> DeleteClassAsync(int id);
    // Phân công GLV cho một lớp
    Task<bool> AssignTeachersAsync(int id, AssignTeachersDto assignTeachersDto);
}