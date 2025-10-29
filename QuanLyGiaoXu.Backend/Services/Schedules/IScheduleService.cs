using QuanLyGiaoXu.Backend.DTOs.ScheduleDtos;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.Schedules;

public interface IScheduleService
{
    Task<List<ScheduleDto>> GetSchedulesAsync();
    Task<ScheduleDto> CreateScheduleAsync(CreateScheduleDto dto);
}