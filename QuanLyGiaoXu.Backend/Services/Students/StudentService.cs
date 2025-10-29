using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.StudentDtos;
using QuanLyGiaoXu.Backend.Entities;
using QuanLyGiaoXu.Backend.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StudentDto>> GetStudentsAsync()
        {
            return await _context.Students
                .OrderBy(s => s.FullName)
                .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Where(s => s.Id == id)
                .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            student.StudentCode = await GenerateStudentCodeAsync();

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return await GetStudentByIdAsync(student.Id);
        }

        public async Task<bool> UpdateStudentAsync(int id, UpdateStudentDto studentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _mapper.Map(studentDto, student);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            if (await _context.ClassEnrollments.AnyAsync(e => e.StudentId == id) ||
                await _context.AttendanceRecords.AnyAsync(ar => ar.StudentId == id))
            {
                throw new InvalidOperationException("Không thể xóa học sinh này vì đã có dữ liệu lịch sử (xếp lớp, điểm danh...). Vui lòng chuyển trạng thái thành 'Đã nghỉ'.");
            }

            _context.Students.Remove(student);
            return await _context.SaveChangesAsync() > 0;
        }

        // === CHỨC NĂNG IMPORT HÀNG LOẠT (ĐÃ ĐƯỢC TÁI CẤU TRÚC) ===
        public async Task<ImportResultDto> ImportStudentsFromExcelAsync(IFormFile file)
        {
            var result = new ImportResultDto();
            var newStudents = new List<Student>();

            var parishDivisionsDict = await GetParishDivisionsDictionaryAsync();
            var nextStudentCodeSequence = await GetNextStudentCodeSequenceAsync();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null) throw new InvalidOperationException("File Excel không hợp lệ hoặc không có sheet nào.");

                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var studentData = ReadStudentDataFromRow(worksheet, row);

                        
                        if (string.IsNullOrWhiteSpace(studentData.FullName) &&
                            string.IsNullOrWhiteSpace(studentData.Dob) &&
                            string.IsNullOrWhiteSpace(studentData.Gender))
                        {
                            continue; 
                        }

                        var validationError = ValidateStudentData(studentData, parishDivisionsDict, row);
                        if (validationError != null)
                        {
                            result.Errors.Add(validationError);
                            continue;
                        }

                        var student = CreateStudentEntity(studentData, parishDivisionsDict);
                        student.StudentCode = $"TC{DateTime.UtcNow:yy}{nextStudentCodeSequence:D4}";
                        nextStudentCodeSequence++;
                        newStudents.Add(student);
                    }
                }
            }

            if (newStudents.Any() && !result.Errors.Any())
            {
                await SaveStudentsToDatabaseAsync(newStudents, result);
            }

            result.FailureCount = result.Errors.Count;
            return result;
        }


        // === CÁC HÀM "HELPER" ĐƯỢC TÁCH NHỎ RA ===

        private (string FullName, string Dob, string Gender, string FatherName, string MotherName, string ParishDivision, string Phone) ReadStudentDataFromRow(ExcelWorksheet worksheet, int row)
        {
            return (
                worksheet.Cells[row, 1].Value?.ToString()?.Trim(),
                worksheet.Cells[row, 2].Value?.ToString()?.Trim(),
                worksheet.Cells[row, 3].Value?.ToString()?.Trim(),
                worksheet.Cells[row, 4].Value?.ToString()?.Trim(),
                worksheet.Cells[row, 5].Value?.ToString()?.Trim(),
                worksheet.Cells[row, 6].Value?.ToString()?.Trim(),
                worksheet.Cells[row, 7].Value?.ToString()?.Trim()
            );
        }

        private string? ValidateStudentData((string FullName, string Dob, string Gender, string FatherName, string MotherName, string ParishDivision, string Phone) data, Dictionary<string, int> parishDivisionsDict, int row)
        {
            if (string.IsNullOrWhiteSpace(data.FullName)) return $"Dòng {row}: Họ và Tên không được để trống.";
            if (!DateTime.TryParse(data.Dob, out _)) return $"Dòng {row}: Ngày sinh '{data.Dob}' không hợp lệ.";
            if (!Enum.TryParse<Genders>(data.Gender, true, out _)) return $"Dòng {row}: Giới tính '{data.Gender}' không hợp lệ. Phải là 'Nam', 'Nu' hoặc 'Khac'.";
            if (!string.IsNullOrWhiteSpace(data.ParishDivision) && !parishDivisionsDict.ContainsKey(data.ParishDivision.ToLowerInvariant()))
                return $"Dòng {row}: Giáo họ '{data.ParishDivision}' không tồn tại trong hệ thống.";

            // Thêm các validate khác nếu cần...

            return null; // Trả về null nếu không có lỗi
        }

        private Student CreateStudentEntity((string FullName, string Dob, string Gender, string FatherName, string MotherName, string ParishDivision, string Phone) data, Dictionary<string, int> parishDivisionsDict)
        {
            DateTime.TryParse(data.Dob, out var dob);
            Enum.TryParse<Genders>(data.Gender, true, out var gender);

            int? parishDivisionId = null;
            if (!string.IsNullOrWhiteSpace(data.ParishDivision))
            {
                parishDivisionId = parishDivisionsDict[data.ParishDivision.ToLowerInvariant()];
            }

            return new Student
            {
                FullName = data.FullName,
                DateOfBirth = dob.Date,
                Gender = gender,
                FatherName = data.FatherName,
                MotherName = data.MotherName,
                ParishDivisionId = parishDivisionId,
                PhoneNumber = data.Phone,
                IsActive = true
            };
        }

        private async Task SaveStudentsToDatabaseAsync(List<Student> students, ImportResultDto result)
        {
            try
            {
                await _context.Students.AddRangeAsync(students);
                await _context.SaveChangesAsync();
                result.SuccessCount = students.Count;
            }
            catch (DbUpdateException ex)
            {
                result.Errors.Add($"Lỗi khi lưu vào CSDL: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private async Task<Dictionary<string, int>> GetParishDivisionsDictionaryAsync()
        {
            return await _context.ParishDivisions
               .ToDictionaryAsync(pd => pd.Name.ToLowerInvariant(), pd => pd.Id);
        }

        private async Task<int> GetNextStudentCodeSequenceAsync()
        {
            var currentYearPrefix = DateTime.UtcNow.ToString("yy");
            var latestStudentInYear = await _context.Students
                .Where(s => s.StudentCode.StartsWith($"TC{currentYearPrefix}"))
                .OrderByDescending(s => s.StudentCode)
                .Select(s => s.StudentCode) // Chỉ lấy cột StudentCode để tối ưu
                .FirstOrDefaultAsync();

            if (latestStudentInYear != null && int.TryParse(latestStudentInYear.AsSpan(4), out int lastSequence))
            {
                return lastSequence + 1;
            }
            return 1;
        }

        private async Task<string> GenerateStudentCodeAsync()
        {
            var nextSequence = await GetNextStudentCodeSequenceAsync();
            return $"TC{DateTime.UtcNow:yy}{nextSequence:D4}";
        }
    }
}