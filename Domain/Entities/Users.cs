using System;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int PhoneNr { get; set; }
    public int Age { get; set; }
    public string? Address { get; set; }
    public Role RoleName { get; set; }
}
