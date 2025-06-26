using coderepo_api.Dtos;

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
        Task<IEnumerable<AlgorithmSummaryDto>> GetAlgorithmsAsync(string? filter, int? userId, bool showPrivates, int page, int pageSize);
    }
}
