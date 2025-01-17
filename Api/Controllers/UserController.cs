using Microsoft.AspNetCore.Mvc;
using SuperFarm.Application.DTOs;
using SuperFarm.Application.Mappers;
using SuperFarm.Infrastructure.Repositories.UserRepositories;

namespace SuperFarm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepositories userRepository) : ControllerBase
    {
        private readonly IUserRepositories _userRepository = userRepository;

        [HttpPost("signup")]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
        {
            try
            {
                var user = await _userRepository.CreateUserAsync(userCreateDto.ToUser());

                return CreatedAtRoute(nameof(GetUserByIdAsync), new { id = user.Id }, user.ToUserDisplayDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetUserByIdAsync")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User with id " + id + " not found");
                }
                return Ok(user.ToUserDisplayDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
        {
            try
            {
                if (id != userUpdateDto.Id)
                {
                    return BadRequest("Ids mismatch");
                }

                var existingUser = await _userRepository.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound("User with id " + id + " not found");
                }

                await _userRepository.UpdateUserAsync(userUpdateDto.ToUser());
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound("User with id " + id + " not found");
                }

                await _userRepository.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUserAsync();
                return Ok(users.Select(u => u.ToUserDisplayDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}