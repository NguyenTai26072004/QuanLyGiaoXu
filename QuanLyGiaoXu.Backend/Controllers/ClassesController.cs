using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyGiaoXu.Backend.DTOs.ClassDtos;
using QuanLyGiaoXu.Backend.DTOs.EnrollmentDtos;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services.Classes;
using QuanLyGiaoXu.Backend.Services.Enrollments;
using System;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IEnrollmentService _enrollmentService;

        public ClassesController(IClassService classService, IEnrollmentService enrollmentService) 
        {
            _classService = classService;
            _enrollmentService = enrollmentService; 
        }

        // GET: /api/classes
        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            return Ok(await _classService.GetClassesAsync());
        }

        // GET: /api/classes/5
        [HttpGet("{id}", Name = "GetClassById")]
        public async Task<IActionResult> GetClassById(int id)
        {
            var classDetail = await _classService.GetClassByIdAsync(id);
            return classDetail == null ? NotFound("Không tìm thấy lớp học.") : Ok(classDetail);
        }

        // POST: /api/classes
        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> CreateClass(CreateClassDto createClassDto)
        {
            try
            {
                var newClass = await _classService.CreateClassAndGenerateSessionsAsync(createClassDto);
                // Dùng CreatedAtRoute thay vì CreatedAtAction
                return CreatedAtRoute("GetClassById", new { id = newClass.Id }, newClass);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: /api/classes/5
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> UpdateClass(int id, UpdateClassDto updateClassDto)
        {
            var result = await _classService.UpdateClassAsync(id, updateClassDto);
            return result ? NoContent() : NotFound("Không tìm thấy lớp học để cập nhật.");
        }

        // DELETE: /api/classes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                var result = await _classService.DeleteClassAsync(id);
                return result ? NoContent() : NotFound("Không tìm thấy lớp học để xóa.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: /api/classes/5/assign-teachers
        [HttpPost("{id}/assign-teachers")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AssignTeachers(int id, AssignTeachersDto assignTeachersDto)
        {
            var result = await _classService.AssignTeachersAsync(id, assignTeachersDto);
            return result ? Ok("Phân công Giáo lý viên thành công.") : NotFound("Không tìm thấy lớp học.");
        }


        // POST: /api/classes/5/enrollments
        [HttpPost("{id}/enrollments")]
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin được xếp lớp
        public async Task<IActionResult> EnrollStudents(int id, EnrollStudentsDto enrollDto)
        {
            try
            {
                await _enrollmentService.EnrollStudentsAsync(id, enrollDto);
                return Ok("Xếp lớp thành công.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}