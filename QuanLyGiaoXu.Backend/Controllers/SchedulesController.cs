using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyGiaoXu.Backend.DTOs.ScheduleDtos;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services.Schedules;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedules()
        {
            return Ok(await _scheduleService.GetSchedulesAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule(CreateScheduleDto dto)
        {
            var schedule = await _scheduleService.CreateScheduleAsync(dto);
            return Ok(schedule);
        }
    }
}