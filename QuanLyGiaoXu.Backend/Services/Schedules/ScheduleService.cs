using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.ScheduleDtos;
using QuanLyGiaoXu.Backend.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.Schedules;

public class ScheduleService : IScheduleService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ScheduleService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ScheduleDto>> GetSchedulesAsync()
    {
        return await _context.Schedules
            .ProjectTo<ScheduleDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ScheduleDto> CreateScheduleAsync(CreateScheduleDto dto)
    {
        var schedule = _mapper.Map<Schedule>(dto);
        await _context.Schedules.AddAsync(schedule);
        await _context.SaveChangesAsync();
        return _mapper.Map<ScheduleDto>(schedule);
    }
}