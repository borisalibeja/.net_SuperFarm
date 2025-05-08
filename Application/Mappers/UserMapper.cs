using SuperFarm.Domain.Entities;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Enums;
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
            PhoneNr = userCreateDto.PhoneNr ?? string.Empty,
            Age = userCreateDto.Age,
            Address = userCreateDto.Address ?? string.Empty
        };
    }

    public static User ToUser(this UserUpdateDto userUpdateDto)
    {
        return new User
        {
            Username = userUpdateDto.Username ?? string.Empty,
            Password = userUpdateDto.Password ?? string.Empty,
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
            PhoneNr = user.PhoneNr ?? string.Empty,
            Age = user.Age ?? 0,
            Address = user.Address,
            Role = user.Role
        };
    }
}