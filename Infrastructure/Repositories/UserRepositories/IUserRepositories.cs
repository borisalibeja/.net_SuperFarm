

using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.UserRepositories;

public interface IUserRepositories
{
    Task<IEnumerable<User>> GetAllUserAsync();
    Task<User?> GetUserByIdAsync(Guid id);

    Task<User> UpdateUserAsync(UserUpdateDto request, Guid? UserId);
    Task DeleteUserAsync(Guid id);
}