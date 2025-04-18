using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
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
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

}
