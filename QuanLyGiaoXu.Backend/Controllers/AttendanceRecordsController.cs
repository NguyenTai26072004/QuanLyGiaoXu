using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.AttendanceRecordDtos;
using QuanLyGiaoXu.Backend.Entities;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services.AttendanceRecords;
using System;
using System.Security.Claims;
using System.Threading.Tasks;


namespace QuanLyGiaoXu.Backend.Controllers.AttendanceRecords
{
    [ApiController]
    [Route("api/sessions/{sessionId}/attendance-records")]
    [Authorize]
    public class AttendanceRecordsController : ControllerBase
    {
        private readonly IAttendanceRecordService _service;
        private readonly ApplicationDbContext _context; // Dùng để kiểm tra quyền
        private readonly UserManager<User> _userManager;

        public AttendanceRecordsController(IAttendanceRecordService service, ApplicationDbContext context, UserManager<User> userManager)
        {
            _service = service;
            _context = context;
            _userManager = userManager;
        }

        // POST: /api/sessions/15/attendance-records
        [HttpPost]
        public async Task<IActionResult> SubmitAttendance(int sessionId, AttendanceSubmissionDto submissionDto)
        {
            try
            {
                var session = await _context.Sessions.FindAsync(sessionId);
                if (session == null) return NotFound("Không tìm thấy buổi học.");

                var classId = session.ClassId;

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole(Roles.Admin.ToString());

                bool isAssignedToClass = false;
                if (!isAdmin)
                {
                    isAssignedToClass = await _context.UserClassAssignments
                       .AnyAsync(uca => uca.ClassId == classId && uca.UserId == currentUserId);
                }

                if (!isAdmin && !isAssignedToClass)
                {
                    return Forbid();
                }

                await _service.SubmitAttendanceAsync(sessionId, submissionDto);
                return Ok("Điểm danh thành công.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi ghi nhận kết quả điểm danh.");
            }
        }

        // GET: /api/sessions/15/attendance-records
        [HttpGet]
        public async Task<IActionResult> GetAttendanceResult(int sessionId)
        {
            try
            {
                // Logic kiểm tra quyền có thể được tái sử dụng tương tự như [HttpPost]
                // nhưng cho phép cả GLV và Admin xem.
                var session = await _context.Sessions.FindAsync(sessionId);
                if (session == null) return NotFound("Không tìm thấy buổi học.");

                var classId = session.ClassId;

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole(Roles.Admin.ToString());
                var isAssignedToClass = await _context.UserClassAssignments
                    .AnyAsync(uca => uca.ClassId == classId && uca.UserId == currentUserId);

                if (!isAdmin && !isAssignedToClass)
                {
                    return Forbid();
                }

                var results = await _service.GetAttendanceResultAsync(sessionId);
                return Ok(results);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}