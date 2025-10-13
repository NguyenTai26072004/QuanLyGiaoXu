using AutoMapper;
using QuanLyGiaoXu.Backend.DTOs.GradeDtos;
using QuanLyGiaoXu.Backend.Entities;

namespace QuanLyGiaoXu.Backend.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // === Cấu hình cho Grade ===
            // Hướng đi: từ Entity Grade -> GradeDto
            CreateMap<Grade, GradeDto>();

            // Hướng đi: từ CreateUpdateGradeDto -> Entity Grade
            CreateMap<CreateUpdateGradeDto, Grade>();
        }
    }
}