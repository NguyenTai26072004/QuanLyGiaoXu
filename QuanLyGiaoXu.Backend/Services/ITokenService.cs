using QuanLyGiaoXu.Backend.Entities;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}