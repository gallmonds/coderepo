using coderepo_api.Dtos;

namespace coderepo_api.Repository.Algorithm
{
    public interface IAlgorithmRepository
    {
        Task<string?> CreateAlgorithm(CreateAlgorithmDto dto, int userId);
        Task<string?> AddLanguage(AddLanguageDto dto, int userId);
        Task<bool> RateAlgorithm(RateContentDto dto);
        Task<bool> CommentAlgorithm(CommentAlgorithmDto dto);
    }
}
