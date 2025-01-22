using System;

namespace SuperFarm.Domain.Entities;

public class Farm
{
    public Guid FarmId { get; set; }
    public Guid UserId { get; set; }
    public string? FarmName { get; set; }
    public string? FarmAddress { get; set; }
    public DateTime CreationDate { get; set; }
}