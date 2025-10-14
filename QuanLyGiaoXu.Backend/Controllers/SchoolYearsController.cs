using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyGiaoXu.Backend.DTOs.SchoolYearDtos;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Yêu cầu đăng nhập
    public class SchoolYearsController : ControllerBase
    {
        private readonly ISchoolYearService _schoolYearService;

        public SchoolYearsController(ISchoolYearService schoolYearService)
        {
            _schoolYearService = schoolYearService;
        }

        [HttpGet] // GET /api/schoolyears
        public async Task<IActionResult> GetSchoolYears()
        {
            return Ok(await _schoolYearService.GetSchoolYearsAsync());
        }

        [HttpPost] // POST /api/schoolyears
        [Authorize(Roles = nameof(Roles.Admin))] 
        public async Task<IActionResult> CreateSchoolYear(CreateUpdateSchoolYearDto schoolYearDto)
        {
            var newSchoolYear = await _schoolYearService.CreateSchoolYearAsync(schoolYearDto);
            
            return Ok(newSchoolYear);
        }

        [HttpPut("{id}")] // PUT /api/schoolyears/5
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> UpdateSchoolYear(int id, CreateUpdateSchoolYearDto schoolYearDto)
        {
            var result = await _schoolYearService.UpdateSchoolYearAsync(id, schoolYearDto);
            return result ? NoContent() : NotFound("Không tìm thấy Năm học để cập nhật.");
        }
    }
}