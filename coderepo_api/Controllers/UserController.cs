using coderepo_api.Dtos;
using coderepo_api.Extensions;
using coderepo_api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace coderepo_api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserProfileAsync(id);

            if (user is null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        [Authorize]
        [HttpPut("users/profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            int userId = User.GetUserId();

            var success = await _userRepository.UpdateUserProfile(userId, dto.Username, dto.Biography);

            if (!success)
                return BadRequest(new { message = "Cannot update profile." });

            return Ok(new { message = "Profile updated successfully." });
        }

        [Authorize]
        [HttpPost("profile/picture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UpdateProfilePictureDto form)
        {
            var userId = User.GetUserId();

            try
            {
                var path = await _userRepository.UpdateProfilePictureAsync(userId, form.File);
                if (path is null)
                    return BadRequest(new { message = "Not a valid image." });

                return Ok(new { profilePic = path });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal error." });
            }
        }
    }
}
