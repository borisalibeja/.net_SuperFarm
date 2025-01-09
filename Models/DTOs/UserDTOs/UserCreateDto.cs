
using SuperFarm.Models.Enums;

namespace SuperFarm.Models.DTOs.UserDTOs
{
    public class UserCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PhoneNr { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }
        public Role RoleName { get; set; }
    }
}