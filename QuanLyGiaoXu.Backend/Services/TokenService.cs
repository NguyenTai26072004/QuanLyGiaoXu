using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuanLyGiaoXu.Backend.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
{
    public class TokenService : ITokenService
    {
        // SymmetricSecurityKey là một loại khóa mà cả hai bên (phát hành và xác thực) đều biết.
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration config, UserManager<User> userManager)
        {
            // Lấy TokenKey từ file appsettings.json và mã hóa nó
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _userManager = userManager;
        }

        public async Task<string> CreateToken(User user)
        {
            // 1. Thêm các "Claims" - thông tin muốn chứa trong token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            // Lấy các vai trò (roles) của user và thêm vào claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 2. Tạo "Credentials" - thông tin xác thực dùng để ký vào token
            // Sử dụng thuật toán mã hóa HmacSha512
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // 3. Mô tả Token - chứa các thông tin như ai phát hành, cho ai, khi nào hết hạn
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), // Token sẽ hết hạn sau 7 ngày
                SigningCredentials = creds
            };

            // 4. Tạo và ghi token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}