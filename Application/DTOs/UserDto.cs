

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Application.DTOs
{
    public class UserCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int PhoneNr { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Role RoleName { get; set; }
    }

    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int PhoneNr { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Role RoleName { get; set; }
    }

    public class UserDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PhoneNr { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Role RoleName { get; set; }
    }
}