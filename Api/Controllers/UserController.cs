using Microsoft.AspNetCore.Mvc;
using SuperFarm.Application.DTOs;
using SuperFarm.Application.Mappers;
using SuperFarm.Infrastructure.Repositories.UserRepositories;

namespace SuperFarm.Api.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController(IUserRepositories userRepository) : ControllerBase
    {
        private readonly IUserRepositories _userRepository = userRepository;


        [HttpGet("{id?}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid? id)
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

        [HttpGet("/myUserInfo")]
        public async Task<IActionResult> GetMyUserInfoAsync()
        {
            try
            {
                var user = await _userRepository.GetMyUserInfoAsync();
                if (user == null)
                {
                    return NotFound("You are not logged in");
                }
                // return Ok(user.ToUserDisplayDto());
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("/searchUser-by-name/{FullName}")]
        public async Task<IActionResult> QueryUserByName(string? FullName)
        {
            try
            {
                var user = await _userRepository.QueryUserByNameAsync(FullName);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{UserId?}")]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto request, Guid? UserId)
        {
            try
            {
                request.UserId = UserId ?? request.UserId;
                var updatedUser = await _userRepository.UpdateUserAsync(request, UserId);
                return Ok(updatedUser);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound("User with id " + id + " not found");
                }

                await _userRepository.DeleteUserAsync(id);
                return Ok("User with id " + id + " deleted successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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