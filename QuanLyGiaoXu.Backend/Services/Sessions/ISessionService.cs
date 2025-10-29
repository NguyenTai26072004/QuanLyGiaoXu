using QuanLyGiaoXu.Backend.DTOs.SessionDtos;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.Sessions;

public interface ISessionService
{
    Task<List<SessionDto>> GetSessionsByClassIdAsync(int classId);
    Task<bool> CancelSessionAsync(int sessionId);
    Task<bool> UpdateSessionAsync(int sessionId, UpdateSessionDto sessionDto);
    Task<SessionDto> CreateManualSessionAsync(CreateManualSessionDto sessionDto);
}