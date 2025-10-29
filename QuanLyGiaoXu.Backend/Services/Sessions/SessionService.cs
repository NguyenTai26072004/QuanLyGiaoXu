using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.SessionDtos;
using QuanLyGiaoXu.Backend.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.Sessions;

public class SessionService : ISessionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SessionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SessionDto>> GetSessionsByClassIdAsync(int classId)
    {
        return await _context.Sessions
            .Where(s => s.ClassId == classId)
            .OrderBy(s => s.SessionDate)
            .ProjectTo<SessionDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> CancelSessionAsync(int sessionId)
    {
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session == null) return false;

        session.IsCancelled = true;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateSessionAsync(int sessionId, UpdateSessionDto sessionDto)
    {
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session == null) return false;

        // Lưu lại ngày cũ nếu đây là lần đầu tiên dời lịch
        if (session.OriginalSessionDate == null && session.SessionDate.Date != sessionDto.SessionDate.Date)
        {
            session.OriginalSessionDate = session.SessionDate;
        }

        session.SessionDate = sessionDto.SessionDate;
        session.Title = sessionDto.Title;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<SessionDto> CreateManualSessionAsync(CreateManualSessionDto sessionDto)
    {
        var session = new Session
        {
            ClassId = sessionDto.ClassId,
            SessionDate = sessionDto.SessionDate,
            Title = sessionDto.Title,
            SessionType = 2 // 2 = Buổi học thủ công/học bù
        };

        await _context.Sessions.AddAsync(session);
        await _context.SaveChangesAsync();
        return _mapper.Map<SessionDto>(session);
    }
}