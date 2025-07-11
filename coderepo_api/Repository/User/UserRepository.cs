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
        private readonly IFileRepository _fileRepository;

        public UserRepository(AppDbContext context, IFileRepository filerepository)
        {
            _context = context;
            _fileRepository = filerepository;
        }

        public async Task<DbUser?> Register(UserRegisterDto dto)
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email);
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

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
                return user;
            }
            catch (PostgresException ex) when (ex.SqlState == "USR01")
            {
                return null;
            }
        }
        public async Task<DbUser?> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            var storedHash = Convert.FromBase64String(user.PasswordHash);
            var storedSalt = Convert.FromBase64String(user.PasswordSalt);

            if (!PasswordUtils.VerifyPassword(password, storedHash, storedSalt))
                return null;

            return user;
        }

        public async Task<DbUser?> GetById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.MediaDb)
                .Where(u => u.auditIsDeleted == '0' && u.Id == userId)
                .Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Biography = u.Biography,
                    CreatedAt = u.CreatedAt,
                    ProfilePic = u.MediaDb.FilePath
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateUserProfile(int userId, string newUsername, string? newBio)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_update_user_profile({0}, {1}, {2})",
                    userId, newUsername, newBio ?? (object)DBNull.Value);

                return true;
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "PER03")
                    Console.WriteLine("The user doesn't have permissions or the user doesn't exist.");
                if (ex.SqlState == "PER10")
                    Console.WriteLine("Username already in use.");

                return false;
            }
        }
        public async Task<string?> UpdateProfilePictureAsync(int userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo se permiten archivos de imagen.");

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var relativePath = Path.Combine("media", "pfp", fileName);

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                await _fileRepository.SaveFileAsync(relativePath, ms.ToArray());
            }

            await _context.Database.ExecuteSqlRawAsync(
                "CALL sp_update_profile_picture({0}, {1}, {2})",
                userId, relativePath, file.ContentType);

            return relativePath;
        }

        public async Task<DbUser?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
