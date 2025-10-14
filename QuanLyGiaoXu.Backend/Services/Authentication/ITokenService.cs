using QuanLyGiaoXu.Backend.Entities;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services.Authentication
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}