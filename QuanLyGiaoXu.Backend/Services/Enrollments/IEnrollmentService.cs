using QuanLyGiaoXu.Backend.DTOs.EnrollmentDtos;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.Enrollments;

public interface IEnrollmentService
{
    Task EnrollStudentsAsync(int classId, EnrollStudentsDto enrollDto);
}