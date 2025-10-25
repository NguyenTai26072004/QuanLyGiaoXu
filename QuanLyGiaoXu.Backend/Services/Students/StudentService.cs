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
using System.Reflection;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
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

            // GỌI HÀM SINH MÃ TỰ ĐỘNG
            student.StudentCode = await GenerateStudentCodeAsync();

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return await GetStudentByIdAsync(student.Id);
        }

        // === PHƯƠNG THỨC SINH MÃ TỰ ĐỘNG ===
        private async Task<string> GenerateStudentCodeAsync()
        {
            // Lấy 2 số cuối của năm hiện tại (ví dụ: 25 từ 2025)
            var currentYearPrefix = DateTime.UtcNow.ToString("yy");

            // Tìm số thứ tự lớn nhất của các sinh viên được tạo trong năm nay
            var latestStudentInYear = await _context.Students
                .Where(s => s.StudentCode.StartsWith($"TC{currentYearPrefix}")) // Tìm các mã bắt đầu bằng TC[YY]
                .OrderByDescending(s => s.StudentCode) // Sắp xếp giảm dần để lấy mã lớn nhất
                .FirstOrDefaultAsync();

            int nextSequence = 1;
            if (latestStudentInYear != null)
            {
                // Nếu đã có sinh viên trong năm, lấy 4 số cuối của mã
                // và chuyển thành số nguyên, sau đó cộng thêm 1
                string lastSequenceStr = latestStudentInYear.StudentCode.Substring(4);
                if (int.TryParse(lastSequenceStr, out int lastSequence))
                {
                    nextSequence = lastSequence + 1;
                }
            }

            // Format số thứ tự thành chuỗi 4 chữ số (ví dụ: 1 -> "0001", 123 -> "0123")
            // và ghép lại thành mã hoàn chỉnh
            return $"TC{currentYearPrefix}{nextSequence:D4}";
        }


        public async Task<bool> UpdateStudentAsync(int id, UpdateStudentDto studentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return false; // Không tìm thấy
            }

            // Cập nhật dữ liệu từ DTO vào entity đã có
            _mapper.Map(studentDto, student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<ImportResultDto> ImportStudentsFromExcelAsync(IFormFile file, int? classId)
        {
            var result = new ImportResultDto();
            var newStudents = new List<Student>();


            // --- Xử lý Logic Sinh mã cho lô import ---
            // Để tránh việc gọi vào CSDL trong vòng lặp, chúng ta sẽ tính trước số thứ tự bắt đầu.
            var currentYearPrefix = DateTime.UtcNow.ToString("yy");
            var latestStudentInYear = await _context.Students
                .Where(s => s.StudentCode.StartsWith($"TC{currentYearPrefix}"))
                .OrderByDescending(s => s.StudentCode)
                .FirstOrDefaultAsync();

            int nextSequence = 1;
            if (latestStudentInYear != null && int.TryParse(latestStudentInYear.StudentCode.Substring(4), out int lastSequence))
            {
                nextSequence = lastSequence + 1;
            }


            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        throw new InvalidOperationException("File Excel không hợp lệ hoặc không có sheet nào.");
                    }

                    var rowCount = worksheet.Dimension.Rows;

                    // Duyệt qua từng dòng của file Excel, bắt đầu từ dòng 2 (bỏ qua tiêu đề)
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            // === ĐỌC DỮ LIỆU TỪ CÁC CỘT ===
                            // Quy ước cột: 1: Họ tên, 2: Ngày sinh, 3: Giới tính, 4: Tên cha, 5: Tên mẹ, 6: Giáo họ, 7: SĐT, 8: id lớp
                            var fullName = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                            var dobStr = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                            var genderStr = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                            var fatherName = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                            var motherName = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                            var parishDivision = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
                            var phoneNumber = worksheet.Cells[row, 7].Value?.ToString()?.Trim();
                            var classIdFromExcelStr = worksheet.Cells[row, 8].Value?.ToString()?.Trim();

                            // === VALIDATE DỮ LIỆU ===
                            if (string.IsNullOrWhiteSpace(fullName))
                            {
                                // Nếu dòng hoàn toàn trống, có thể là cuối file, bỏ qua
                                if (string.IsNullOrWhiteSpace(dobStr) && string.IsNullOrWhiteSpace(genderStr)) continue;
                                result.Errors.Add($"Dòng {row}: Họ và Tên không được để trống.");
                                continue;
                            }

                            if (!DateTime.TryParse(dobStr, out DateTime dob))
                            {
                                result.Errors.Add($"Dòng {row}: Ngày sinh '{dobStr}' không hợp lệ.");
                                continue;
                            }

                            if (!Enum.TryParse<Genders>(genderStr, true, out Genders gender))
                            {
                                result.Errors.Add($"Dòng {row}: Giới tính '{genderStr}' không hợp lệ. Phải là 'Nam' hoặc 'Nu' hoặc 'Khac'.");
                                continue;
                            }
                            int? finalClassId = classId; 

                            if (!finalClassId.HasValue && !string.IsNullOrWhiteSpace(classIdFromExcelStr))
                            {
                                // Nếu không có classId từ API, thì mới thử đọc từ Excel
                                if (int.TryParse(classIdFromExcelStr, out int parsedClassId))
                                {
                                    finalClassId = parsedClassId;
                                }
                                else
                                {
                                    result.Errors.Add($"Dòng {row}: Mã Lớp '{classIdFromExcelStr}' trong file Excel không phải là một con số hợp lệ.");
                                    continue;
                                }
                            }

                            // === TẠO ĐỐI TƯỢNG STUDENT ===
                            var student = new Student
                            {
                                FullName = fullName,
                                DateOfBirth = dob.Date, // Chỉ lấy ngày, không lấy giờ
                                Gender = gender,
                                FatherName = fatherName,
                                MotherName = motherName,
                                ParishDivision = parishDivision,
                                PhoneNumber = phoneNumber,
                                ClassId = finalClassId, // Ưu tiên gán vào lớp được chọn trên giao diện
                                IsActive = true,
                            };

                            // Gán mã tự động theo số thứ tự đã tính
                            student.StudentCode = $"TC{currentYearPrefix}{nextSequence:D4}";
                            nextSequence++; // Tăng số thứ tự cho người tiếp theo

                            newStudents.Add(student);
                        }
                        catch (Exception ex)
                        {
                            // Ghi lại lỗi nếu có bất kỳ sự cố không lường trước nào khi xử lý một dòng
                            result.Errors.Add($"Dòng {row}: Đã xảy ra lỗi không xác định - {ex.Message}");
                        }
                    }
                }
            }

            // === LƯU VÀO CSDL ===
            // Chỉ thực hiện lưu nếu không có lỗi nào được ghi nhận trong toàn bộ file
            if (newStudents.Any() && !result.Errors.Any())
            {
                try
                {
                    await _context.Students.AddRangeAsync(newStudents);
                    await _context.SaveChangesAsync();
                    result.SuccessCount = newStudents.Count;
                }
                catch (DbUpdateException ex)
                {
                    // Bắt lỗi từ CSDL (ví dụ: trùng lặp mã nếu có lỗi logic)
                    result.Errors.Add($"Lỗi khi lưu vào cơ sở dữ liệu: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            result.FailureCount = result.Errors.Count;
            return result;
        }
    }
}