using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.DTOs.ParishDivisionDtos;
using QuanLyGiaoXu.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace QuanLyGiaoXu.Backend.Services.ParishDivisions;

public class ParishDivisionService : IParishDivisionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ParishDivisionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ParishDivisionDto>> GetParishDivisionsAsync()
    {
        return await _context.ParishDivisions
            .OrderBy(pd => pd.Name)
            .ProjectTo<ParishDivisionDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ParishDivisionDto> CreateParishDivisionAsync(CreateUpdateParishDivisionDto dto)
    {
        var parishDivision = _mapper.Map<ParishDivision>(dto);
        await _context.ParishDivisions.AddAsync(parishDivision);
        await _context.SaveChangesAsync();
        return _mapper.Map<ParishDivisionDto>(parishDivision);
    }

    public async Task<bool> UpdateParishDivisionAsync(int id, CreateUpdateParishDivisionDto dto)
    {
        var parishDivision = await _context.ParishDivisions.FindAsync(id);
        if (parishDivision == null) return false;

        _mapper.Map(dto, parishDivision);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteParishDivisionAsync(int id)
    {
        var parishDivision = await _context.ParishDivisions.FindAsync(id);
        if (parishDivision == null) return false;

        // Kiểm tra ràng buộc: không cho xóa nếu có học sinh đang thuộc về giáo họ này
        if (await _context.Students.AnyAsync(s => s.ParishDivisionId == id))
        {
            throw new InvalidOperationException("Không thể xóa Giáo họ này vì đang có học sinh thuộc về nó.");
        }

        _context.ParishDivisions.Remove(parishDivision);
        return await _context.SaveChangesAsync() > 0;
    }
}