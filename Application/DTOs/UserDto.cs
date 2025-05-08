

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Application.DTOs
{
    public class UserCreateDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNr { get; set; }
        public string? Address { get; set; }

    }

    public class UserUpdateDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public string? PhoneNr { get; set; }
        public string? Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }
    }

    public class UserDisplayDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNr { get; set; }
        public string? Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }

    }

    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}