using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyGiaoXu.Backend.DTOs.SessionDtos;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services.Sessions;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // Lấy tất cả session của một lớp 
        // GET: /api/sessions?classId=5
        [HttpGet]
        public async Task<IActionResult> GetSessions([FromQuery] int classId)
        {
            return Ok(await _sessionService.GetSessionsByClassIdAsync(classId));
        }

        // Hủy một buổi học
        // PATCH: /api/sessions/10/cancel
        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> CancelSession(int id)
        {
            var result = await _sessionService.CancelSessionAsync(id);
            return result ? NoContent() : NotFound();
        }

        // Sửa thông tin buổi học (dời lịch, đặt tiêu đề)
        // PUT: /api/sessions/10
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> UpdateSession(int id, UpdateSessionDto sessionDto)
        {
            var result = await _sessionService.UpdateSessionAsync(id, sessionDto);
            return result ? NoContent() : NotFound();
        }

        // Thêm một buổi học bù (không theo lịch)
        // POST: /api/sessions/manual
        [HttpPost("manual")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> CreateManualSession(CreateManualSessionDto sessionDto)
        {
            var session = await _sessionService.CreateManualSessionAsync(sessionDto);
            return Ok(session);
        }
    }
}