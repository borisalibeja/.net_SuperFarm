using Microsoft.AspNetCore.Mvc;
using SuperFarm.Infrastructure.Repositories.UserRepositories;
using SuperFarm.Services;

namespace SuperFarm.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly IUserRepositories _userRepository;

        public AuthController(IUserRepositories userRepository, JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !VerifyPassword(request.Password, user.Password))
            {
                return Unauthorized();
            }

            var token = _jwtTokenService.GenerateToken(user.Id.ToString(), user.RoleName.ToString());
            return Ok(new { Token = token });
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            // Implement your password verification logic here
            // For example, use a hashing library to compare the hashed password
            return inputPassword == storedHash; // Replace this with your actual verification logic
        }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}