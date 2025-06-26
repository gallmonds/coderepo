using coderepo_api.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
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
                    .Where(a => a.OwnerId == userId && a.Title == dto.Title)
                    .OrderByDescending(a => a.Id)
                    .FirstOrDefaultAsync();

                if (newAlgorithm == null)
                    return null;

                var algorithmMeta = await _context.AlgorithmMetas
                    .FirstOrDefaultAsync(m => m.AlgorithmId == newAlgorithm.Id);

                if (algorithmMeta == null)
                    return null;

                return algorithmMeta.RootPath;
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
                    .Where(l => l.AlgorithmId == dto.p_algorithm_id && l.SupportedLangId == dto.p_supportedlang_id)
                    .OrderByDescending(l => l.CreatedAt)
                    .FirstOrDefaultAsync();
                if (newLang == null)
                {
                    return null;
                }

                var algorithmChangelog = await _context.AlgorithmChangelogs
                    .Where(c => c.AlgorithmId == dto.p_algorithm_id)
                    .OrderByDescending(c => c.CreatedAt)
                    .FirstOrDefaultAsync();

                if (algorithmChangelog == null)
                {
                    return null;
                }

                return algorithmChangelog.FilePath;
            }
            catch (PostgresException)
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
                    dto.ContentId, userId, dto.Body, dto.ReplyToId!);
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

        public async Task<bool> AddCollaborators(List<int> userIds, int algorithmId, int currentUserId)
        {
            try
            {
                var userArray = userIds.ToArray();

                var parameters = new[]
                {
                    new NpgsqlParameter("p_users", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer) { Value = userArray },
                    new NpgsqlParameter("p_dbuser_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = currentUserId },
                    new NpgsqlParameter("p_algorithm_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = algorithmId }
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_add_collaborators(@p_users, @p_dbuser_id, @p_algorithm_id)", parameters);

                return true;
            }
            catch (PostgresException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<AlgorithmSummaryDto>> GetAlgorithmsAsync(string? filter, int? viewerUserId, int? userId, bool showPrivates, int page, int pageSize)
        {
            var query = _context.Algorithms
                .Include(a => a.UserDb).ThenInclude(u => u.MediaDb)
                .Include(a => a.AlgorithmLangs).ThenInclude(al => al.SupportedLang).ThenInclude(sl => sl.MediaDb)
                .Include(a => a.AlgorithmTags).ThenInclude(at => at.Tag)
                .Include(a => a.AlgorithmMeta)
                .Include(a => a.AlgorithmCollaborators)
                .Where(a => a.AuditIsDeleted == '0' && a.AlgorithmMeta.IsFlagged != '1' && a.AlgorithmMeta.IsDisabled != '1');

            if (!showPrivates)
            {
                query = query.Where(a => a.AlgorithmMeta.IsPrivate != '1');
            }
            else if (viewerUserId.HasValue)
            {
                query = query.Where(a => a.AlgorithmMeta.IsPrivate != '1' ||
                                         a.OwnerId == viewerUserId ||
                                         a.AlgorithmCollaborators.Any(c => c.UserId == viewerUserId));
            }
            else
            {
                query = query.Where(a => a.AlgorithmMeta.IsPrivate != '1');
            }

            if (filter == "user_algorithms" && userId.HasValue)
            {
                query = query.Where(a => a.OwnerId == userId.Value);
            }

            switch (filter)
            {
                case "most_popular":
                    var recentDate = DateTime.UtcNow.AddDays(-30);
                    query = query
                        .Where(a => a.CreatedAt >= recentDate)
                        .OrderByDescending(a => a.Ratings.Count(r => r.AuditIsDeleted == '0' && r.ContentId == a.Id));
                    break;
                case "most_recent":
                    query = query.OrderByDescending(a => a.CreatedAt);
                    break;
                case "most_rated":
                    query = query.OrderByDescending(a => a.Ratings.Count(r => r.AuditIsDeleted == '0' && r.ContentId == a.Id));
                    break;
                case "user_algorithms":
                    query = query.OrderByDescending(a => a.CreatedAt);
                    break;
                default:
                    query = query.OrderByDescending(a => a.CreatedAt);
                    break;
            }

            return await query.AsSplitQuery()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AlgorithmSummaryDto
                {
                    AlgorithmId = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    CreatedAt = a.CreatedAt,
                    Owner = new UserSummaryDto
                    {
                        Username = a.UserDb.Username,
                        ProfilePic = a.UserDb.MediaDb.FilePath
                    },
                    RatingCount = _context.Ratings.Count(r => r.AuditIsDeleted == '0' && r.ContentId == a.Id),
                    CommentCount = _context.Comments.Count(c => c.AuditIsDeleted == '0' && c.ContentId == a.Id),
                    Tags = a.AlgorithmTags.Select(at => at.Tag.Name).Take(5).ToList(),
                    Languages = a.AlgorithmLangs.Select(al => new LanguageSummaryDto
                    {
                        LangName = al.SupportedLang.LangName,
                        IconPath = al.SupportedLang.MediaDb.FilePath
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<AlgorithmDetailDto?> GetAlgorithmDetailsAsync(int algorithmId, int? viewerUserId)
        {
            var algorithm = await _context.Algorithms
                .Include(a => a.UserDb).ThenInclude(u => u.MediaDb)
                .Include(a => a.AlgorithmMeta)
                .Include(a => a.AlgorithmLangs).ThenInclude(al => al.SupportedLang).ThenInclude(sl => sl.MediaDb)
                .Include(a => a.AlgorithmTags).ThenInclude(at => at.Tag)
                .Include(a => a.AlgorithmCollaborators)
                .FirstOrDefaultAsync(a => a.Id == algorithmId);

            if (algorithm == null)
                return null;

            var meta = algorithm.AlgorithmMeta;
            if (algorithm.AuditIsDeleted == '1' || meta.IsFlagged == '1' || meta.IsDisabled == '1')
                return null;

            if (meta.IsPrivate == '1' &&
                viewerUserId != algorithm.OwnerId &&
                !algorithm.AlgorithmCollaborators.Any(c => c.UserId == viewerUserId))
                return null;

            var rootComments = await _context.Comments
                .Include(c => c.Owner).ThenInclude(u => u.MediaDb)
                .Where(c => c.TypeId == 2 && c.ContentId == algorithmId && c.AuditIsDeleted == '0' && c.ReplyToId == null)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    CommentId = c.Id,
                    Content = c.Body,
                    Date = c.CreatedAt,
                    Username = c.Owner.Username,
                    PfpRoute = c.Owner.MediaDb.FilePath,
                    RatingCount = _context.Ratings.Count(r =>
                        r.TypeId == 1 && r.ContentId == c.Id && r.AuditIsDeleted == '0')
                })
                .ToListAsync();

            return new AlgorithmDetailDto
            {
                AlgorithmId = algorithm.Id,
                Title = algorithm.Title,
                Description = algorithm.Description,
                CreatedAt = algorithm.CreatedAt,
                Owner = new UserSummaryDto
                {
                    Username = algorithm.UserDb.Username,
                    ProfilePic = algorithm.UserDb.MediaDb.FilePath
                },
                RatingCount = await _context.Ratings.CountAsync(r =>
                    r.TypeId == 2 && r.ContentId == algorithmId && r.AuditIsDeleted == '0'),
                Tags = algorithm.AlgorithmTags.Select(at => at.Tag.Name).ToList(),
                Languages = algorithm.AlgorithmLangs.Select(al => new LanguageDetailDto
                {
                    LangName = al.SupportedLang.LangName,
                    IconPath = al.SupportedLang.MediaDb.FilePath,
                    CodeletPath = "codelet/" + al.RootlangPath
                }).ToList(),
                Comments = rootComments
            };
        }


    }
}
