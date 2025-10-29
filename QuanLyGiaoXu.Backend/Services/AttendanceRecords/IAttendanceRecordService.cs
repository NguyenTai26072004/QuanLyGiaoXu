using QuanLyGiaoXu.Backend.DTOs.AttendanceRecordDtos;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.AttendanceRecords;

public interface IAttendanceRecordService
{
    // Nhận dữ liệu điểm danh hàng loạt cho một buổi học cụ thể
    Task SubmitAttendanceAsync(int sessionId, AttendanceSubmissionDto submissionDto);
    Task<List<AttendanceResultDto>> GetAttendanceResultAsync(int sessionId);
}