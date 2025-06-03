using coderepo_api.Dtos;

namespace coderepo_api.Repository.Algorithm
{
    public interface IAlgorithmRepository
    {
        Task<string?> CreateAlgorithm(CreateAlgorithmDto dto, int userId);
    }
}
