using System;

namespace SuperFarm.Domain.Entities;

public class Farm
{
    public int UserId { get; set; }
    public string? FarmName { get; set; }
    public string? FarmAddress { get; set; }
    public DateTime CreationDate { get; set; }
}