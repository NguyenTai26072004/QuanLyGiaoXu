using AutoMapper;
using QuanLyGiaoXu.Backend.DTOs.AccountDtos;
using QuanLyGiaoXu.Backend.DTOs.AttendanceRecordDtos;
using QuanLyGiaoXu.Backend.DTOs.ClassDtos;
using QuanLyGiaoXu.Backend.DTOs.GradeDtos;
using QuanLyGiaoXu.Backend.DTOs.ParishDivisionDtos;
using QuanLyGiaoXu.Backend.DTOs.ScheduleDtos;    
using QuanLyGiaoXu.Backend.DTOs.SchoolYearDtos;
using QuanLyGiaoXu.Backend.DTOs.SessionDtos;      
using QuanLyGiaoXu.Backend.DTOs.StudentDtos;
using QuanLyGiaoXu.Backend.Entities;
using System.Linq;

namespace QuanLyGiaoXu.Backend.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<Grade, GradeDto>().ReverseMap(); 
            CreateMap<CreateUpdateGradeDto, Grade>();
            
            CreateMap<SchoolYear, SchoolYearDto>().ReverseMap();
            CreateMap<CreateUpdateSchoolYearDto, SchoolYear>();
            
            // THÊM MAP CHO SCHEDULE (Khuôn mẫu Lịch)
            CreateMap<Schedule, ScheduleDto>() 
                 .ForMember(d => d.SchoolYearName, o => o.MapFrom(s => s.SchoolYear.Year));
            CreateMap<CreateScheduleDto, Schedule>(); 
            
            // === CẤU HÌNH CHO USER / ACCOUNT ===
            CreateMap<User, AccountDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.HasValue ? src.Gender.Value.ToString() : null));

            // === CẤU HÌNH CHO STUDENT ===
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Enrollments.FirstOrDefault(e => e.EndDate == null).Class.ClassName))
                .ForMember(dest => dest.ParishDivisionName, opt => opt.MapFrom(src => src.ParishDivision.Name));

            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>()
                 .ForMember(d => d.StudentCode, o => o.Ignore());

            // === CẤU HÌNH CHO CLASS ===
            CreateMap<Class, ClassDetailDto>()
                 .ForMember(d => d.Name, o => o.MapFrom(s => s.ClassName))
                 .ForMember(d => d.GradeName, o => o.MapFrom(s => s.Grade.Name))
                 .ForMember(d => d.ScheduleName, o => o.MapFrom(s => string.Join(", ", s.ClassSchedules.Select(cs => cs.Schedule.Name))))
                 .ForMember(d => d.TeacherNames, o => o.MapFrom(s => s.UserClassAssignments.Select(ct => ct.User.FullName).ToList()))
                 .ForMember(d => d.NumberOfStudents, o => o.MapFrom(s => s.Enrollments.Count(e => e.EndDate == null)));


            // === CẤU HÌNH CHO ĐIỂM DANH  ===
            CreateMap<Session, SessionDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));


            // === CẤU HÌNH CHO KẾT QUẢ ĐIỂM DANH  ===
            CreateMap<AttendanceRecord, AttendanceResultDto>()
                .ForMember(d => d.StudentCode, o => o.MapFrom(s => s.Student.StudentCode))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Student.FullName))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.Student.DateOfBirth))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            // === Cấu hình cho ParishDivision ===
            CreateMap<ParishDivision, ParishDivisionDto>();
            CreateMap<CreateUpdateParishDivisionDto, ParishDivision>();
        }
    }
}