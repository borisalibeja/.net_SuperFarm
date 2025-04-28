using SuperFarm.Domain.Entities;
using SuperFarm.Application.DTOs;
namespace SuperFarm.Application.Mappers;

public static class UserMapper
{
    public static User ToUser(this UserCreateDto userCreateDto)
    {
        return new User
        {
            Username = userCreateDto.Username,
            Password = userCreateDto.Password,
            FirstName = userCreateDto.FirstName ?? string.Empty,
            LastName = userCreateDto.LastName ?? string.Empty,
            Email = userCreateDto.Email ?? string.Empty,
            PhoneNr = userCreateDto.PhoneNr,
            Age = userCreateDto.Age,
            Address = userCreateDto.Address ?? string.Empty
        };
    }

    public static User ToUser(this UserUpdateDto userUpdateDto)
    {
        return new User
        {
            UserId = userUpdateDto.UserId,
            Username = userUpdateDto.Username,
            Password = userUpdateDto.Password,
            FirstName = userUpdateDto.FirstName,
            LastName = userUpdateDto.LastName,
            Age = userUpdateDto.Age,
            Email = userUpdateDto.Email,
            PhoneNr = userUpdateDto.PhoneNr,
            Address = userUpdateDto.Address,
            Role = userUpdateDto.Role
        };
    }

    public static UserDisplayDto ToUserDisplayDto(this User user)
    {
        return new UserDisplayDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Password = user.Password,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNr = user.PhoneNr ?? 0,
            Age = user.Age ?? 0,
            Address = user.Address,
            Role = user.Role
        };
    }
}