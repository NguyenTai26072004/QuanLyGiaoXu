using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.AttendanceRecordDtos;
using QuanLyGiaoXu.Backend.Entities;
using System.Linq;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.AttendanceRecords;



public class AttendanceRecordService : IAttendanceRecordService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AttendanceRecordService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task SubmitAttendanceAsync(int sessionId, AttendanceSubmissionDto submissionDto)
    {
        var session = await _context.Sessions
                                .Include(s => s.Class)
                                .ThenInclude(c => c.Enrollments)
                                .FirstOrDefaultAsync(s => s.Id == sessionId);
        if (session == null)
        {
            throw new KeyNotFoundException("Không tìm thấy buổi học.");
        }

        var studentIdsInClass = session.Class.Enrollments
                                      .Where(e => e.EndDate == null) 
                                      .Select(e => e.StudentId).ToList();

        var existingRecords = await _context.AttendanceRecords
            .Where(ar => ar.SessionId == sessionId)
            .ToListAsync();

        foreach (var recordDto in submissionDto.Records)
        {
            // Chỉ xử lý học sinh thuộc lớp này
            if (!studentIdsInClass.Contains(recordDto.StudentId)) continue;

            var existingRecord = existingRecords.FirstOrDefault(ar => ar.StudentId == recordDto.StudentId);

            if (existingRecord != null)
            {
                // Cập nhật
                existingRecord.Status = recordDto.Status;
                existingRecord.Notes = recordDto.Notes;
            }
            else
            {
                // Tạo mới
                _context.AttendanceRecords.Add(new AttendanceRecord
                {
                    SessionId = sessionId,
                    StudentId = recordDto.StudentId,
                    Status = recordDto.Status,
                    Notes = recordDto.Notes
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    // --- THÊM PHƯƠNG THỨC MỚI ---
    public async Task<List<AttendanceResultDto>> GetAttendanceResultAsync(int sessionId)
    {
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session == null)
        {
            throw new KeyNotFoundException("Không tìm thấy buổi học.");
        }

        return await _context.AttendanceRecords
            .Where(ar => ar.SessionId == sessionId)
            .OrderBy(ar => ar.Student.FullName)
            .ProjectTo<AttendanceResultDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}