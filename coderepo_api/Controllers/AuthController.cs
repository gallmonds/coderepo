using coderepo_api.Dtos;
using coderepo_api.Repository;
using coderepo_api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace coderepo_api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase {
        private readonly IUserRepository _userRepository;
        private readonly JwtUtils _jwtUtils;

        public AuthController(IUserRepository userRepository, JwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var user = await _userRepository.Register(dto);
            if (user == null)
                return BadRequest("Username or email already exists.");

            var token = _jwtUtils.GenerateJwtToken(user);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _userRepository.Login(dto.Username, dto.Password);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            var token = _jwtUtils.GenerateJwtToken(user);

            return Ok(new { token });
        }
    }
}
