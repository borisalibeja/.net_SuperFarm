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
            Username = userCreateDto.Username ?? string.Empty,
            Password = userCreateDto.Password ?? string.Empty,
            FirstName = userCreateDto.FirstName ?? string.Empty,
            LastName = userCreateDto.LastName ?? string.Empty,
            Email = userCreateDto.Email ?? string.Empty,
            UserPhoneNr = userCreateDto.UserPhoneNr ?? string.Empty,
            CountryCode = userCreateDto.CountryCode,
            Age = userCreateDto.Age,
            StreetName = userCreateDto.StreetName,
            City = userCreateDto.City,
            Country = userCreateDto.Country,
            County = userCreateDto.County,
            BuildingNr = userCreateDto.BuildingNr,
            FloorNr = userCreateDto.FloorNr,
            PostCode = userCreateDto.PostCode,
            ProfileImgUrl = userCreateDto.ProfileImgUrl
        };
    }

    public static User ToUser(this UserUpdateDto userUpdateDto)
    {
        return new User
        {
            Password = userUpdateDto.Password ?? string.Empty,
            FirstName = userUpdateDto.FirstName,
            LastName = userUpdateDto.LastName,
            Age = userUpdateDto.Age,
            Email = userUpdateDto.Email,
            UserPhoneNr = userUpdateDto.UserPhoneNr,
            CountryCode = userUpdateDto.CountryCode,
            Role = userUpdateDto.Role,
            StreetName = userUpdateDto.StreetName,
            City = userUpdateDto.City,
            Country = userUpdateDto.Country,
            County = userUpdateDto.County,
            BuildingNr = userUpdateDto.BuildingNr,
            FloorNr = userUpdateDto.FloorNr,
            PostCode = userUpdateDto.PostCode,
            ProfileImgUrl = userUpdateDto.ProfileImgUrl
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
            UserPhoneNr = user.UserPhoneNr ?? string.Empty,
            CountryCode = user.CountryCode,
            Age = user.Age ?? 0,
            Role = user.Role,
            StreetName = user.StreetName,
            City = user.City,
            Country = user.Country,
            County = user.County,
            BuildingNr = user.BuildingNr,
            FloorNr = user.FloorNr,
            PostCode = user.PostCode,
            ProfileImgUrl = user.ProfileImgUrl,
            UserCreatedAt = user.UserCreatedAt,
            UserUpdatedAt = user.UserUpdatedAt
        };
    }
}