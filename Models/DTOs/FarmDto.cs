using System;

namespace SuperFarm.Models.DTOs.FarmDTOs;

public class FarmCreateDto
{
    public int user_id { get; set; }
    public string farm_name { get; set; } = string.Empty;
    public string farm_address { get; set; } = string.Empty;
    public DateTime creation_date { get; set; }

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

