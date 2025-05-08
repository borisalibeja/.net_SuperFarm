

using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.UserRepositories;

public interface IUserRepositories
{
    Task<IEnumerable<User>> GetAllUserAsync();
    Task<User?> GetUserByIdAsync(Guid? id);
    Task<User?> GetMyUserInfoAsync();
    Task<IEnumerable<UserDisplayDto>> QueryUserByNameAsync(string? name);
    Task<User> UpdateUserAsync(UserUpdateDto request, Guid? UserId);
    Task DeleteUserAsync(Guid id);
}