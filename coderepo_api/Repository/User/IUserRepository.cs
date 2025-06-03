using coderepo_api.Dtos;
using coderepo_api.Models;

namespace coderepo_api.Repository
{
    public interface IUserRepository
    {
        Task<AppUser?> Register(UserRegisterDto dto);
        Task<AppUser?> Login(string username, string password);
        Task<AppUser?> GetById(int userId);

    }
}
