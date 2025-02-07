using Microsoft.AspNetCore.Mvc;
using SuperFarm.Application.DTOs;
using SuperFarm.Application.Mappers;
using SuperFarm.Domain.Entities;
using SuperFarm.Infrastructure.Repositories.FarmRepositories;
using SuperFarm.Services;


namespace SuperFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly IFarmRepositories _farmRepository;
        private readonly UserContextService _userContextService;

        public FarmController(IFarmRepositories farmRepository, UserContextService userContextService)
        {
            _farmRepository = farmRepository;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFarm(FarmCreateDto request)
        {
            try
            {
                var farm = await _farmRepository.CreateFarmAsync(request);

                return CreatedAtRoute(nameof(GetFarmByIdAsync), new { FarmId = farm.ToFarmDisplayDto() }, farm.ToFarmDisplayDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{FarmId}", Name = "GetFarmByIdAsync")]
        public async Task<IActionResult> GetFarmByIdAsync(Guid FarmId)
        {
            try
            {
                var farm = await _farmRepository.GetFarmByIdAsync(FarmId);
                if (farm == null)
                {
                    return NotFound("Farm with id " + FarmId + " not found");
                }
                return Ok(farm);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{FarmId?}")]
        public async Task<IActionResult> UpdateFarmAsync(FarmUpdateDto request, Guid? FarmId)
        {
            var userId = _userContextService.GetUserId();
            var userRole = _userContextService.GetUserRole();

            try
            {
                request.FarmId = FarmId ?? request.FarmId;
                var updatedFarm = await _farmRepository.UpdateFarmAsync(request, FarmId);
                return Ok(updatedFarm);
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
        public async Task<IActionResult> DeleteFarmAsync(Guid id)
        {
            try
            {
                var existingFarm = await _farmRepository.GetFarmByIdAsync(id);
                if (existingFarm == null)
                {
                    return NotFound("Farm with id " + id + " not found");
                }
                await _farmRepository.DeleteFarmAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFarm()
        {
            try
            {
                var farm = await _farmRepository.GetAllFarmAsync();
                return Ok(farm);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
