using System;

namespace SuperFarm.Models.DTOs.FarmDTOs;

public class FarmCreateDto
{
    public int UserId { get; set; }
    public string FarmName { get; set; } = string.Empty;
    public string FarmAddress { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }

}
public class FarmUpdateDto
{
    public int UserId { get; set; }
    public string FarmName { get; set; } = string.Empty;
    public string FarmAddress { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
}
public class FarmDisplayDto
{
    public int UserId { get; set; }
    public string FarmName { get; set; } = string.Empty;
    public string FarmAddress { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
}

