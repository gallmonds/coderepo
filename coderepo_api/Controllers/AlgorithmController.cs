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

        [HttpPost("create")]
        public async Task<IActionResult> CreateAlgorithm(CreateAlgorithmDto dto)
        {
            int userId = User.GetUserId();

            var result = await _repository.CreateAlgorithm(dto, userId);
            if (result == null)
                return StatusCode(403, new { message = "User lacks permission to create algorithms." });

            await _fileRepository.CreateDirectoryAsync(result);

            return Ok(result);
        }

        [HttpPost("addlang")]
        public async Task<IActionResult> AddLanguage([FromForm] AddLanguageFormDto form)
        {
            int userId = User.GetUserId();

            var dto = new AddLanguageDto
            {
                p_algorithm_id = form.AlgorithmId,
                p_supportedlang_id = form.SupportedLangId,
                p_dbuser_id = userId
            };

            var result = await _repository.AddLanguage(dto, userId);

            if (result == null)
                return StatusCode(403, new { message = "User lacks permission to add languages." });

            if (form.File == null || form.File.Length == 0)
                return BadRequest(new { message = "No file was uploaded." });

            var filePath = result;
            //var dirPath = Path.GetDirectoryName(filePath);

            //if (dirPath == null)
            //    return BadRequest(new { message = "Invalid file path." });

            //dirPath = Path.Combine("codelet/", dirPath);
            filePath = Path.Combine("/app/static/codelet/", filePath);

            //await _fileRepository.CreateDirectoryAsync(dirPath);

            using (var ms = new MemoryStream())
            {
                await form.File.CopyToAsync(ms);
                await _fileRepository.SaveFileAsync(filePath, ms.ToArray());
            }

            return Ok(new { path = filePath });
        }

        [HttpPost("rate")]
        public async Task<IActionResult> RateAlgorithm(RateAlgorithmFormDto formdto)
        {
            int userId = User.GetUserId();

            var dto = new RateContentDto
            {
                ContentId = formdto.AlgorithmId,
                TypeId = 2,
                UserId = userId
            };

            var result = await _repository.RateAlgorithm(dto);

            if (!result)
                return StatusCode(403, new { message = "User lacks permission to rate algorithms." });

            return Ok(new { message = "Submitted." });
        }

        [HttpPost("comment")]
        public async Task<IActionResult> CommentAlgorithm(CommentAlgorithmDto)
        {
            int userId = User.GetUserId();
            var dto = new CommentAlgorithmDto
            {
                ContentId = formdto.AlgorithmId,
                TypeId = 2,
                UserId = userId,
                Comment = formdto.Comment
            };
            var result = await _repository.CommentAlgorithm(dto);
            if (!result)
                return StatusCode(403, new { message = "User lacks permission to comment on algorithms." });
            return Ok(new { message = "Comment submitted." });
        }

    }
}
