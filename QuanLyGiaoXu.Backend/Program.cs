// =================================================================================
// SECTION 1: USING DIRECTIVES
// =================================================================================
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using QuanLyGiaoXu.Backend.Data;
using QuanLyGiaoXu.Backend.Entities;
using QuanLyGiaoXu.Backend.Services;
using QuanLyGiaoXu.Backend.Services.AttendanceRecords;
using QuanLyGiaoXu.Backend.Services.Authentication;
using QuanLyGiaoXu.Backend.Services.Classes;
using QuanLyGiaoXu.Backend.Services.Enrollments;
using QuanLyGiaoXu.Backend.Services.Grades;
using QuanLyGiaoXu.Backend.Services.ParishDivisions;
using QuanLyGiaoXu.Backend.Services.Schedules;
using QuanLyGiaoXu.Backend.Services.Sessions;
using QuanLyGiaoXu.Backend.Services.Students;
using System.Text;


// =================================================================================
// SECTION 2: APPLICATION BUILDER SETUP & SERVICE CONFIGURATION
// =================================================================================

var builder = WebApplication.CreateBuilder(args);


// --- A. Dịch vụ cơ bản của ASP.NET Core ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "QuanLyGiaoXu API",
        Description = "Hệ thống API quản lý Thiếu nhi Giáo xứ Tiên Chu"
    });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Nhập 'Bearer' [dấu cách] và token.\r\n\r\nVí dụ: \"Bearer 12345abcdef\""
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// --- B. Dịch vụ tùy chỉnh của ứng dụng ---
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ISchoolYearService, SchoolYearService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IAttendanceRecordService, AttendanceRecordService>();
builder.Services.AddScoped<IEnrollmentService,  EnrollmentService>();
builder.Services.AddScoped<IParishDivisionService, ParishDivisionService>();



// --- C. Dịch vụ Xác thực & Phân quyền ---
builder.Services.AddIdentity<User, IdentityRole>(options => 
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    // Cấu hình scheme mặc định là JWT
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});


// =================================================================================
// SECTION 3: BUILD THE APP & RUN SEEDING
// Xây dựng app và chạy các tác vụ khởi tạo TRƯỚC KHI web server khởi động.
// =================================================================================

var app = builder.Build();

// Gọi hàm Seed Database ngay sau khi app được build.
// Chương trình sẽ đợi (await) cho đến khi hàm này thực hiện xong.
await SeedDatabaseAsync(app);


// =================================================================================
// SECTION 4: HTTP REQUEST PIPELINE CONFIGURATION
// =================================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// =================================================================================
// SECTION 5: RUN THE APPLICATION
// =================================================================================
app.Run();


// =================================================================================
// SECTION 6: HELPER METHOD FOR SEEDING
// Hàm tách riêng để thực hiện việc Migrate và Seed Data.
// =================================================================================
async Task SeedDatabaseAsync(IHost app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync(); // Luôn đợi migrate xong
        await Seed.SeedUsers(userManager, roleManager); // Sau đó mới seed
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration or seeding the data.");
    }
}