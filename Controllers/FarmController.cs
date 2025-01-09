using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperFarm.Mappers;
using SuperFarm.Models.DTOs;
using SuperFarm.Repositories;

namespace SuperFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly IFarmRepositories _farmRepository;

        public FarmController(IFarmRepositories farmRepository)
        {
            _farmRepository = farmRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFarm(FarmCreateDto farmCreateDto)
        {
            try
            {
                var farm = await _farmRepository.CreateFarmAsync(farmCreateDto.ToFarm());

                return CreatedAtRoute(nameof(GetFarmByIdAsync), new { id = farm.Id }, farm.ToFarmDisplayDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetFarmByIdAsync")]
        public async Task<IActionResult> GetFarmByIdAsync(int id)
        {
            try
            {
                var farm = await _farmRepository.GetFarmByIdAsync(id);
                if (farm == null)
                {
                    return NotFound("Farm with id " + id + " not found");
                }
                return Ok(farm);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFarmAsync(int id, FarmUpdateDto farmUpdateDto)
        {
            try
            {
                if (id != farmUpdateDto.Id)
                {
                    return BadRequest("Ids mismatch");
                }
                var existingFarm = await _farmRepository.GetFarmByIdAsync(id);
                if (existingFarm == null)
                {
                    return NotFound("Farm with id " + id + " not found");
                }
                await _farmRepository.UpdateFarmAsync(farmUpdateDto.ToFarm());
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFarmAsync(int id)
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
