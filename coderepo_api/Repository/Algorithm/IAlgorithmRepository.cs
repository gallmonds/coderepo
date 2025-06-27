using coderepo_api.Dtos;
using coderepo_api.Models;

namespace coderepo_api.Repository.Algorithm
{
    public interface IAlgorithmRepository
    {
        Task<string?> CreateAlgorithm(CreateAlgorithmDto dto, int userId);
        Task<string?> AddLanguage(AddLanguageDto dto, int userId);
        Task<bool> RateContent(RateContentDto dto, int userId);
        Task<bool> CommentAlgorithm(CommentAlgorithmDto dto, int userId);
        Task<bool> UpdateAlgorithm(CreateAlgorithmDto dto, int userId, int algorithmId);
        Task<bool> DeleteAlgorithm(int algorithmId, int userId);
        Task<bool> AddCollaborators(List<int> userIds, int algorithmId, int currentUserId);
        Task<IEnumerable<AlgorithmSummaryDto>> GetAlgorithmsAsync(string? filter, int? viewerUserId, int? userId, bool showPrivates, int page, int pageSize);
        Task<AlgorithmDetailDto?> GetAlgorithmDetailsAsync(int algorithmId, int? viewerUserId);
        Task<List<CommentDto>> GetRepliesAsync(int parentCommentId);
        Task<IEnumerable<AlgorithmSummaryDto>> SearchAlgorithmsAsync(string? searchQuery, int? viewerUserId, bool showPrivates, int page, int pageSize);
        Task<bool> AssignTagsAsync(AssignTagsDto dto, int userId);
        Task<bool> CreateTagsAsync(IEnumerable<string> tagNames, int userId);
    }
}
