using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyGiaoXu.Backend.DTOs.ClassDtos;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services;
using QuanLyGiaoXu.Backend.Services.Classes;
using System;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route sẽ là /api/classes
    [Authorize] // Bắt buộc đăng nhập cho tất cả các API trong controller này
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        // === READ APIs ===
        [HttpGet] // GET: /api/classes
        public async Task<IActionResult> GetClasses()
        {
            return Ok(await _classService.GetClassesAsync());
        }

        [HttpGet("{id}", Name = "GetClassById")] // GET: /api/classes/5
        public async Task<IActionResult> GetClassById(int id)
        {
            var aClass = await _classService.GetClassByIdAsync(id);
            return aClass == null ? NotFound("Không tìm thấy lớp học.") : Ok(aClass);
        }

        // === CREATE API ===
        [HttpPost] // POST: /api/classes
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin được tạo
        public async Task<IActionResult> CreateClass(CreateClassDto createClassDto)
        {
            var newClass = await _classService.CreateClassAsync(createClassDto);

            // Lấy lại thông tin đầy đủ của lớp vừa tạo để trả về (có GradeName,...)
            var classDto = await _classService.GetClassByIdAsync(newClass.Id);

            // Trả về response 201 Created cùng với thông tin và URL của lớp mới
            return CreatedAtAction(nameof(GetClassById), new { id = classDto.Id }, classDto);
        }

        // === UPDATE API ===
        [HttpPut("{id}")] // PUT: /api/classes/5
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin được cập nhật
        public async Task<IActionResult> UpdateClass(int id, UpdateClassDto updateClassDto)
        {
            var result = await _classService.UpdateClassAsync(id, updateClassDto);
            return result ? NoContent() : NotFound("Không tìm thấy lớp học để cập nhật."); // 204 No Content
        }

        // === DELETE API ===
        [HttpDelete("{id}")] // DELETE: /api/classes/5
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin được xóa
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                var result = await _classService.DeleteClassAsync(id);
                return result ? NoContent() : NotFound("Không tìm thấy lớp học để xóa.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Trả về lỗi nghiệp vụ 400 Bad Request
            }
        }

        // === NGHIỆP VỤ PHÂN CÔNG GIÁO LÝ VIÊN ===
        [HttpPost("{id}/assign-teachers")] // POST: /api/classes/5/assign-teachers
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin được phân công
        public async Task<IActionResult> AssignTeachersToClass(int id, AssignTeacherDto assignTeacherDto)
        {
            var result = await _classService.AssignTeachersToClassAsync(id, assignTeacherDto);
            return result ? Ok("Phân công Giáo lý viên thành công.") : NotFound("Không tìm thấy lớp học.");
        }
    }
}