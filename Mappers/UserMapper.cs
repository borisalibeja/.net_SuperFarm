using SuperFarm.Models;
using SuperFarm.Models.DTOs.UserDTOs;

namespace SuperFarm.Mappers;

public static class UserMapper
{
    public static User ToUser(UserCreateDto userCreateDto)
    {
        return new User
        {
            Id = userCreateDto.Id,
            Name = userCreateDto.Name,
            Surname = userCreateDto.Surname,
            Email = userCreateDto.Email,
            PhoneNr = userCreateDto.PhoneNr,
            Age = userCreateDto.Age,
            Address = userCreateDto.Address,
            RoleName = userCreateDto.RoleName,
        };
    }

    public static User ToUser(UserUpdateDto userUpdateDto)
    {
        return new User
        {
            Id = userUpdateDto.Id,
            Name = userUpdateDto.Name,
            Surname = userUpdateDto.Surname,
            Email = userUpdateDto.Email,
            PhoneNr = userUpdateDto.PhoneNr,
            Age = userUpdateDto.Age,
            Address = userUpdateDto.Address,
            RoleName = userUpdateDto.RoleName,
        };
    }

    public static User ToUser(UserDisplayDto userDisplayDto)
    {
        return new User
        {
            Id = userDisplayDto.Id,
            Name = userDisplayDto.Name,
            Surname = userDisplayDto.Surname,
            Email = userDisplayDto.Email,
            PhoneNr = userDisplayDto.PhoneNr,
            Age = userDisplayDto.Age,
            Address = userDisplayDto.Address,
            RoleName = userDisplayDto.RoleName,
        };
    }
}