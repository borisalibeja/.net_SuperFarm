
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.FarmRepositories;

public interface IFarmRepositories
{

    Task<IEnumerable<Farm>> GetAllFarmAsync();
    Task<Farm?> GetFarmByIdAsync(Guid FarmId);
    Task<Farm> GetMyFarmAsync();
    Task<Farm> CreateFarmAsync(FarmCreateDto request);
    Task<Farm> UpdateFarmAsync(FarmUpdateDto farmUpdate, FarmDisplayDto farmDisplay, Guid? FarmId);
    Task DeleteFarmAsync(Guid? FarmId);
    Task DeleteMyFarmAsync();

    Task<IEnumerable<FarmDisplayDto>> QueryFarmByNameAsync(string? farmName);
}

