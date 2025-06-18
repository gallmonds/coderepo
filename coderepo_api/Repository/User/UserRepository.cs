using coderepo_api.Dtos;
using coderepo_api.Models;
using coderepo_api.Utils;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace coderepo_api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> Register(UserRegisterDto dto)
        {
            var exists = await _context.Users
                .AnyAsync(u => u.username == dto.Username || u.email == dto.Email);
            if (exists)
                return null;

            PasswordUtils.CreatePasswordHash(dto.Password, out var hashBytes, out var saltBytes);

            string hash = Convert.ToBase64String(hashBytes);
            string salt = Convert.ToBase64String(saltBytes);

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL create_user({0}, {1}, {2}, {3})",
                    dto.Username, dto.Email, hash, salt);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.username == dto.Username);
                return user;
            }
            catch (PostgresException ex) when (ex.SqlState == "USR01")
            {
                return null;
            }
        }
        public async Task<AppUser?> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            if (user == null) return null;

            var storedHash = Convert.FromBase64String(user.password_hash);
            var storedSalt = Convert.FromBase64String(user.password_salt);

            if (!PasswordUtils.VerifyPassword(password, storedHash, storedSalt))
                return null;

            return user;
        }

        public async Task<AppUser?> GetById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.user_id == userId);
        }

    }
}
