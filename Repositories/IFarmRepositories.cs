using SuperFarm.Models;

namespace SuperFarm.Repositories;

public interface IFarmRepositories
{

    Task<IEnumerable<Farm>> GetAllFarmAsync();
    Task<Farm?> GetFarmByIdAsync(int id);
    Task<Farm> CreateFarmAsync(Farm farm);
    Task<Farm> UpdateFarmAsync(Farm farm);
    Task DeleteFarmAsync(int id);
}
