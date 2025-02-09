using Microsoft.AspNetCore.Mvc;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;
using SuperFarm.Services;

namespace SuperFarm.Api.Controllers
{

    [ApiController]
    [Route("/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result is null)
                return BadRequest("Invalid username or password.");

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserCreateDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result is null)
                return BadRequest("Username already exists.");

            return Ok(result);
        }
    }

}