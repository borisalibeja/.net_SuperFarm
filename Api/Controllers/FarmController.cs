using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperFarm.Application.DTOs;
using SuperFarm.Application.Mappers;
using SuperFarm.Infrastructure.Repositories.FarmRepositories;
using SuperFarm.Services;
using Swashbuckle.AspNetCore.Annotations;


namespace SuperFarm.Controllers
{
    [Route("/[controller]")]
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

        [HttpGet("/myFarmInfo")]
        public async Task<IActionResult> GetMyFarm()
        {
            try
            {
                var farm = await _farmRepository.GetMyFarmAsync();
                if (farm == null)
                {
                    return NotFound("Farm not found");
                }
                return Ok(farm);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Authorize(Policy = "FarmerPolicy")]
        [HttpPut("{FarmId?}")]
        public async Task<IActionResult> UpdateFarm([FromBody] FarmUpdateDto farmUpdate,
        [FromQuery] FarmDisplayDto farmDisplay,
        [FromRoute] Guid? FarmId)
        {

            try
            {
                farmDisplay.FarmId = FarmId ?? farmDisplay.FarmId;
                var updatedFarm = await _farmRepository.UpdateFarmAsync(farmUpdate, farmDisplay, FarmId);
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

        [Authorize(Policy = "FarmerPolicy")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFarm([FromQuery] Guid? FarmId)
        {

            await _farmRepository.DeleteFarmAsync(FarmId);
            return Ok("Farm Account deleted successfully");

        }

        [HttpDelete("/delete-my-farm")]
        public async Task<IActionResult> DeleteMyFarm()
        {
            try
            {
                await _farmRepository.DeleteMyFarmAsync();
                return Ok("Farm Account deleted successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
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


        [HttpGet("/queryFarm-by-name/{FarmName}")]
        public async Task<IActionResult> QueryUserByName(string? FarmName)
        {
            try
            {
                var farm = await _farmRepository.QueryFarmByNameAsync(FarmName);
                if (farm == null)
                {
                    return NotFound(new { message = "Farm not found." });
                }

                return Ok(farm);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
