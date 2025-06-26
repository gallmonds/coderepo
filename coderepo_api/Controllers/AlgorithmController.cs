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
            filePath = Path.Combine("codelet/", filePath);

            //await _fileRepository.CreateDirectoryAsync(dirPath);

            using (var ms = new MemoryStream())
            {
                await form.File.CopyToAsync(ms);
                await _fileRepository.SaveFileAsync(filePath, ms.ToArray());
            }

            return Ok(new { path = filePath });
        }

        [HttpPost("rate")]
        public async Task<IActionResult> RateContent(RateContentDto dto)
        {
            int userId = User.GetUserId();

            var result = await _repository.RateContent(dto, userId);

            if (!result)
                return StatusCode(403, new { message = "User lacks permission to rate algorithms." });

            return Ok(new { message = "Submitted." });
        }

        [HttpPost("comment")]
        public async Task<IActionResult> CommentAlgorithm(CommentAlgorithmDto dto)
        {
            int userId = User.GetUserId();

            var result = await _repository.CommentAlgorithm(dto, userId);
            if (!result)
                return StatusCode(403, new { message = "User lacks permission to comment on algorithms." });
            return Ok(new { message = "Comment submitted." });
        }

        [HttpPut("update/{algorithmId}")]
        public async Task<IActionResult> UpdateAlgorithm(int algorithmId, [FromBody] CreateAlgorithmDto dto)
        {
            int userId = User.GetUserId();
            var result = await _repository.UpdateAlgorithm(dto, userId, algorithmId);
            if (!result)
                return StatusCode(403, new { message = "User lacks permission to update algorithms." });
            return Ok(new { message = "Algorithm updated successfully." });
        }

        [HttpPut("delete/{algorithmId}")]
        public async Task<IActionResult> DeleteAlgorithm(int algorithmId)
        {
            int userId = User.GetUserId();
            var result = await _repository.DeleteAlgorithm(algorithmId, userId);
            if (!result)
                return StatusCode(403, new { message = "User lacks permission to delete algorithms." });
            return Ok(new { message = "Algorithm deleted successfully." });
        }

        // >:(
        [HttpPost("add-collaborators")]
        public async Task<IActionResult> AddCollaborators([FromBody] AddCollaboratorsDto dto)
        {
            int userId = User.GetUserId();

            var result = await _repository.AddCollaborators(dto.UserIds, dto.AlgorithmId, userId);


            if (!result)
            {
                return StatusCode(403, new { message = "User lacks permission to add collaborators." });
            }

            return Ok(new { message = "Collaborators added successfully." });
        }


        [AllowAnonymous]
        [HttpGet("summary")]
        public async Task<IActionResult> GetAlgorithms(string? filter, int? userId, bool showPrivates = false, int page = 1, int pageSize = 10)
        {
            int? viewerUserId = null;
            try { viewerUserId = User.GetUserId(); } catch { }

            var algorithms = await _repository.GetAlgorithmsAsync(filter, viewerUserId, userId, showPrivates, page, pageSize);
            return Ok(algorithms);
        }

        [AllowAnonymous]
        [HttpGet("algorithms/{id}")]
        public async Task<IActionResult> GetAlgorithmDetails(int id)
        {
            int? viewerUserId = null;
            try { viewerUserId = User.GetUserId(); } catch { }

            var algorithm = await _repository.GetAlgorithmDetailsAsync(id, viewerUserId);
            if (algorithm == null)
                return NotFound(new { message = "Algorithm not found or access denied." });

            return Ok(algorithm);
        }

        [AllowAnonymous]
        [HttpGet("comments/{commentId}/replies")]
        public async Task<IActionResult> GetCommentReplies(int commentId)
        {
            var replies = await _repository.GetRepliesAsync(commentId);

            if (replies == null || !replies.Any())
                return NotFound(new { message = "No replies found or comment does not exist." });

            return Ok(replies);
        }
    }
}
