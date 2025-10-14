using AutoMapper;
using QuanLyGiaoXu.Backend.DTOs.ClassDtos;
using QuanLyGiaoXu.Backend.DTOs.GradeDtos;
using QuanLyGiaoXu.Backend.DTOs.SchoolYearDtos;
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


            // === Cấu hình cho Class ===
            // Ánh xạ đặc biệt: lấy GradeName từ Grade.Name
            CreateMap<Class, ClassDto>()
                .ForMember(dest => dest.SchoolYearName, opt => opt.MapFrom(src => src.SchoolYear.Year))
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name))
                .ForMember(dest => dest.TeacherNames, opt => opt.MapFrom(src => src.UserClassAssignments.Select(uca => uca.User.FullName).ToList()))
                .ForMember(dest => dest.NumberOfStudents, opt => opt.MapFrom(src => src.Students.Count));

            CreateMap<CreateClassDto, Class>();
            CreateMap<UpdateClassDto, Class>();


            // === Cấu hình cho SchoolYear ===
            CreateMap<SchoolYear, SchoolYearDto>();
            CreateMap<CreateUpdateSchoolYearDto, SchoolYear>();
        }
    }
}