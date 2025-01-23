

using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.UserRepositories;

public interface IUserRepositories
{
    Task<IEnumerable<User>> GetAllUserAsync();
    Task<User?> GetUserByIdAsync(Guid id);

    Task<User?> GetUserByEmailAsync(string email);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
}