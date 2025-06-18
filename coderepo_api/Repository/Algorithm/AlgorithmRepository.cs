using coderepo_api.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

    }
}
