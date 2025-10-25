using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyGiaoXu.Backend.DTOs.StudentDtos;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services;
using System;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: /api/students
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetStudentsAsync();
            return Ok(students);
        }

        // GET: /api/students/5
        [HttpGet("{id}", Name = "GetStudentById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            return student == null ? NotFound("Không tìm thấy học sinh.") : Ok(student);
        }

        // POST: /api/students
        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin))] 
        public async Task<IActionResult> CreateStudent(CreateStudentDto createStudentDto)
        {
            try
            {
                var newStudent = await _studentService.CreateStudentAsync(createStudentDto);
                return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id }, newStudent);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        // PUT: /api/students/5
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))] 
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto updateStudentDto)
        {
            try
            {
                var result = await _studentService.UpdateStudentAsync(id, updateStudentDto);
                return result ? NoContent() : NotFound("Không tìm thấy học sinh để cập nhật.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: /api/students/5
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))] 
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            return result ? NoContent() : NotFound("Không tìm thấy học sinh để xóa.");
        }


        // POST: /api/students/bulk-import
        [HttpPost("bulk-import")]
        [Authorize(Roles = nameof(Roles.Admin))] // Chỉ Admin được import
        public async Task<IActionResult> ImportStudents(IFormFile file, [FromQuery] int? classId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Vui lòng chọn một file Excel.");
            }

            var result = await _studentService.ImportStudentsFromExcelAsync(file, classId);

            if (result.FailureCount > 0)
            {
                // Trả về 200 OK nhưng kèm theo thông báo lỗi để frontend hiển thị
                return Ok(result);
            }

            return Ok(result);
        }
    }
}