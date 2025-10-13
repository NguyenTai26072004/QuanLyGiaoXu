using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyGiaoXu.Backend.DTOs.GradeDtos;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services;
using System;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Tất cả các API trong đây đều yêu cầu đăng nhập
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpGet] // GET api/grades
        public async Task<IActionResult> GetGrades()
        {
            return Ok(await _gradeService.GetGradesAsync());
        }

        [HttpGet("{id}")] // GET api/grades/5
        public async Task<IActionResult> GetGradeById(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            return grade == null ? NotFound() : Ok(grade);
        }

        [HttpPost] // POST api/grades
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin mới được tạo
        public async Task<IActionResult> CreateGrade(CreateUpdateGradeDto createDto)
        {
            var newGrade = await _gradeService.CreateGradeAsync(createDto);
            return CreatedAtAction(nameof(GetGradeById), new { id = newGrade.Id }, newGrade);
        }

        [HttpPut("{id}")] // PUT api/grades/5
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin mới được sửa
        public async Task<IActionResult> UpdateGrade(int id, CreateUpdateGradeDto updateDto)
        {
            var result = await _gradeService.UpdateGradeAsync(id, updateDto);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")] // DELETE api/grades/5
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin mới được xóa
        public async Task<IActionResult> DeleteGrade(int id)
        {
            try
            {
                var result = await _gradeService.DeleteGradeAsync(id);
                return result ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
