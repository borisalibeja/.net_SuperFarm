

using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.UserRepositories;

public interface IUserRepositories
{
    Task<IEnumerable<User>> GetAllUserAsync();
    Task<User?> GetUserByIdAsync(int id);

    Task<User?> GetUserByEmailAsync(string email);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}