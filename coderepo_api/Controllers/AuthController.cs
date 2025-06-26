using coderepo_api.Dtos;
using coderepo_api.Repository;
using coderepo_api.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
            var emailRegex = new Regex(
                @"(?:[a-z0-9!#$%&'*+\=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\=?^_`{|}~-]+)*|\""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*\"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)])",
                RegexOptions.IgnoreCase
            );

            if (!emailRegex.IsMatch(dto.Email))
                return BadRequest("Invalid email format.");

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
