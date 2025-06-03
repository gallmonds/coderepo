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

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyUserData()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null)
                return Unauthorized("No user id in token.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return BadRequest("Invalid user id in token.");

            var user = await _userRepository.GetById(userId);
            if (user == null)
                return NotFound("User not found.");

            return Ok(new
            {
                user.user_id,
                user.username,
                user.email,
                user.pfp_id,
                user.isflagged,
                user.isbanned,
                user.audit_isdeleted
            });
        }
    }
}
