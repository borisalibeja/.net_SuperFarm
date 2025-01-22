
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.FarmRepositories;

public interface IFarmRepositories
{

    Task<IEnumerable<Farm>> GetAllFarmAsync();
    Task<Farm?> GetFarmByIdAsync(Guid id);
    Task<Farm> CreateFarmAsync(Farm farm);
    Task<Farm> UpdateFarmAsync(Farm farm);
    Task DeleteFarmAsync(Guid id);
}
