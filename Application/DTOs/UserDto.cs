

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Application.DTOs
{
    public class UserCreateDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public int PhoneNr { get; set; }
        public string? Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }
    }

    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public int PhoneNr { get; set; }
        public string? Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }
    }

    public class UserDisplayDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public int PhoneNr { get; set; }
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