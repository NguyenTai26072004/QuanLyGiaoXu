using QuanLyGiaoXu.Backend.DTOs.StudentDtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<StudentDto> CreateStudentAsync(CreateStudentDto studentDto);
        Task<bool> UpdateStudentAsync(int id, UpdateStudentDto studentDto);
        Task<bool> DeleteStudentAsync(int id);
        Task<ImportResultDto> ImportStudentsFromExcelAsync(IFormFile file, int? classId);
    }
}