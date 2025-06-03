using coderepo_api.Dtos;
using coderepo_api.Extensions;
using coderepo_api.Repository;
using coderepo_api.Repository.Algorithm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderepo_api.Controllers
{
    [ApiController]
    [Route("api/algorithms")]
    [Authorize]
    public class AlgorithmController : ControllerBase
    {
        private readonly IAlgorithmRepository _repository;
        private readonly IFileRepository _fileRepository;

        public AlgorithmController(IAlgorithmRepository repository, IFileRepository filerepository)
        {
            _repository = repository;
            _fileRepository = filerepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlgorithm(CreateAlgorithmDto dto)
        {
            int userId = User.GetUserId();


            var result = await _repository.CreateAlgorithm(dto, userId);
            if (result == null)
                return StatusCode(403, new { message = "User lacks permission to create algorithms." });

            await _fileRepository.CreateDirectoryAsync(result);

            return Ok(result);
        }
    }
}
