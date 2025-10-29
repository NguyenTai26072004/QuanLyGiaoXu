using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.DTOs.AccountDtos;
using QuanLyGiaoXu.Backend.Entities;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services.Authentication;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    // Route cơ bản cho controller này sẽ là: /api/account
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        // "Tiêm" các dịch vụ cần thiết vào qua constructor
        public AccountController(UserManager<User> userManager, ITokenService tokenService, IMapper mapper) 
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper; // << THÊM DÒNG NÀY
        }

        // --- ENDPOINT ĐỂ TẠO TÀI KHOẢN MỚI ---
        //POST: /api/account/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // 1. Kiểm tra xem Username đã tồn tại chưa
            if (await UserExists(registerDto.Username))
            {
                // Trả về lỗi 400 Bad Request nếu username đã được sử dụng
                return BadRequest("Username is taken");
            }

            // 2. Tạo đối tượng User mới
            var user = new User
            {
                UserName = registerDto.Username.ToLower(), // Luôn lưu username dạng chữ thường
                FullName = registerDto.FullName
            };

            // 3. Dùng UserManager để tạo user trong CSDL, kèm theo mật khẩu
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                // Nếu có lỗi (VD: mật khẩu quá yếu), trả về lỗi
                return BadRequest(result.Errors);
            }

            // 4. Gán vai trò mặc định là "GLV" cho người dùng mới
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.GLV.ToString());
            if (!roleResult.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // 5. Nếu tạo thành công, trả về thông tin user kèm theo token
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                FullName = user.FullName
            };
        }

        // --- ENDPOINT ĐỂ ĐĂNG NHẬP ---
        // Route sẽ là: POST /api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // 1. Tìm user theo username
            var user = await _userManager.Users
                                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            // 2. Nếu không tìm thấy user, trả về lỗi 401 Unauthorized (Không được phép)
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            // 3. Kiểm tra mật khẩu
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) // Nếu result là false
            {
                return Unauthorized("Invalid password");
            }

            // 4. Nếu đăng nhập thành công, trả về thông tin user kèm token
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                FullName = user.FullName
            };
        }

        // GET: /api/account
        [HttpGet]
        [Authorize(Roles = nameof(Roles.Admin))] 
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetUsers()
        {
            // Lấy tất cả user từ CSDL
            var users = await _userManager.Users.ToListAsync();

            // Chuẩn bị một danh sách để chứa kết quả DTO
            var usersToReturn = new List<AccountDto>();

            foreach (var user in users)
            {
                // Với mỗi user, map sang AccountDto
                var userDto = _mapper.Map<AccountDto>(user);
                // Lấy danh sách vai trò của user đó
                userDto.Roles = await _userManager.GetRolesAsync(user);

                usersToReturn.Add(userDto);
            }

            return Ok(usersToReturn);
        }




        // --- Phương thức Helper (hỗ trợ) ---
        private async Task<bool> UserExists(string username)
        {
            // Kiểm tra trong CSDL xem có user nào có UserName trùng không (không phân biệt hoa/thường)
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}