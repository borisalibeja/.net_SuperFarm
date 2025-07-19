

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Application.DTOs
{
    public class UserCreateDto
    {
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNr { get; set; }
        public string? CountryCode { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? County { get; set; }
        public string? BuildingNr { get; set; }
        public string? FloorNr { get; set; }
        public string? PostCode { get; set; }
        public string? ProfileImgUrl { get; set; }
        public Role? Role { get; set; }

    }

    public class UserUpdateDto
    {
        public string? Password { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNr { get; set; }
        public string? CountryCode { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? County { get; set; }
        public string? BuildingNr { get; set; }
        public string? FloorNr { get; set; }
        public string? PostCode { get; set; }
        public string? ProfileImgUrl { get; set; }


        public Role Role { get; set; }
    }

    public class UserDisplayDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string? UserEmail { get; set; } = string.Empty;
        public string? UserPhoneNr { get; set; }
        public string? CountryCode { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? County { get; set; }
        public string? BuildingNr { get; set; }
        public string? FloorNr { get; set; }
        public string? PostCode { get; set; }
        public string? ProfileImgUrl { get; set; }

        public Role Role { get; set; }
        public DateTime? UserCreatedAt { get; set; }
        public DateTime? UserUpdatedAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }

    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}