using coderepo_api.Dtos;
using coderepo_api.Models;

namespace coderepo_api.Repository
{
    public interface IUserRepository
    {
        Task<DbUser?> Register(UserRegisterDto dto);
        Task<DbUser?> Login(string username, string password);
        Task<DbUser?> GetById(int userId);
        Task<UserProfileDto?> GetUserProfileAsync(int userId);
        Task<bool> UpdateUserProfile(int userId, string newUsername, string? newBio);
        Task<string?> UpdateProfilePictureAsync(int userId, IFormFile file);
    }
}
