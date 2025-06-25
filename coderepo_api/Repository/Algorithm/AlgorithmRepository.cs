using coderepo_api.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.IO;


namespace coderepo_api.Repository.Algorithm
{
    public class AlgorithmRepository : IAlgorithmRepository
    {
        private readonly AppDbContext _context;

        public AlgorithmRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string?> CreateAlgorithm(CreateAlgorithmDto dto, int userId)
        {
            try
            {
                var isPrivateChar = dto.IsPrivate ? '1' : '0';

                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_create_algorithm({0}, {1}, {2}, {3})",
                    dto.Title, dto.Description, isPrivateChar, userId);

                var newAlgorithm = await _context.Algorithms
                    .Where(a => a.owner_id == userId && a.title == dto.Title)
                    .OrderByDescending(a => a.algorithm_id)
                    .FirstOrDefaultAsync();

                if (newAlgorithm == null)
                    return null;

                var algorithmMeta = await _context.AlgorithmMeta
                    .FirstOrDefaultAsync(m => m.algorithm_id == newAlgorithm.algorithm_id);

                if (algorithmMeta == null)
                    return null;

                return algorithmMeta.root_path;
            }
            catch (PostgresException ex) when (ex.SqlState == "PER01")
            {
                return null;
            }
        }

        public async Task<string?> AddLanguage(AddLanguageDto dto, int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_add_language({0}, {1}, {2})",
                    dto.p_algorithm_id, dto.p_dbuser_id, dto.p_supportedlang_id);

                var newLang = await _context.AlgorithmLangs
                    .Where(l => l.algorithm_id == dto.p_algorithm_id && l.lang_id == dto.p_supportedlang_id)
                    .OrderByDescending(l => l.created_at)
                    .FirstOrDefaultAsync();
                if (newLang == null)
                {
                    return null;
                }

                var algorithmChangelog = await _context.AlgorithmChangelogs
                    .Where(c => c.algorithm_id == dto.p_algorithm_id)
                    .OrderByDescending(c => c.created_at)
                    .FirstOrDefaultAsync();

                if (algorithmChangelog == null)
                {
                    return null;
                }

                return algorithmChangelog.file_path;
            }
            catch (PostgresException ex)
            {
                return null;
            }
        }

        public async Task<bool> RateContent(RateContentDto dto, int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                "CALL sp_rate_content({0}, {1}, {2})",
                dto.ContentId, userId, dto.TypeId);
                return true;
            }
            catch (PostgresException)
            {
                return false;
            }
        }

        public async Task<bool> CommentAlgorithm(CommentAlgorithmDto dto, int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_comment({0}, {1}, {2}, {3})",
                    dto.ContentId, userId, dto.Body, dto.ReplyToId);
                return true;
            }
            catch (PostgresException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAlgorithm(CreateAlgorithmDto dto, int userId, int algorithmId)
        {
            try
            {
                var isPrivateChar = dto.IsPrivate ? '1' : '0';

                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_update_algorithm({0}, {1}, {2}, {3}, {4})",
                    algorithmId, userId, dto.Title, dto.Description, isPrivateChar);
                return true;
            }
            catch (PostgresException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAlgorithm(int algorithmId, int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_delete_algorithm({0}, {1})",
                    algorithmId, userId);
                return true;
            }
            catch (PostgresException)
            {
                return false;
            }
        }
    }
}
