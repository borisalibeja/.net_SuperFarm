
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.FarmRepositories;

public interface IFarmRepositories
{

    Task<IEnumerable<Farm>> GetAllFarmAsync();
    Task<Farm?> GetFarmByIdAsync(Guid FarmId);
    Task<Farm> GetFarmByUserIdAsync();
    Task<Farm> CreateFarmAsync(FarmCreateDto request);
    Task<Farm> UpdateFarmAsync(FarmUpdateDto request, Guid? FarmId);
    Task DeleteFarmAsync(Guid id);
}

