using SuperFarm.Models;
using SuperFarm.Models.DTOs.UserDTOs;

namespace SuperFarm.Mappers;

public static class UserMapper
{
    public static User ToUser(this UserCreateDto userCreateDto)
    {
        return new User
        {
            Name = userCreateDto.Name,
            Surname = userCreateDto.Surname,
            Email = userCreateDto.Email,
            Password = userCreateDto.Password,
            PhoneNr = userCreateDto.PhoneNr,
            Age = userCreateDto.Age,
            Address = userCreateDto.Address,
            RoleName = userCreateDto.RoleName,
        };
    }

    public static User ToUser(this UserUpdateDto userUpdateDto)
    {
        return new User
        {
            Id = userUpdateDto.Id,
            Name = userUpdateDto.Name,
            Surname = userUpdateDto.Surname,
            Email = userUpdateDto.Email,
            Password = userUpdateDto.Password,
            PhoneNr = userUpdateDto.PhoneNr,
            Age = userUpdateDto.Age,
            Address = userUpdateDto.Address,
            RoleName = userUpdateDto.RoleName,
        };
    }

    public static UserDisplayDto ToUserDisplayDto(this User user)
    {
        return new UserDisplayDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNr = user.PhoneNr,
            Age = user.Age,
            Address = user.Address,
            RoleName = user.RoleName,
        };
    }
}