using System;

namespace SuperFarm.Application.DTOs;

public class FarmCreateDto
{
    public Guid FarmId { get; set; }
    public Guid UserId { get; set; }
    public string? FarmName { get; set; }
    public string? FarmAddress { get; set; }
    public DateTime CreationDate { get; set; }

}
public class FarmUpdateDto
{
    public Guid FarmId { get; set; }
    public Guid UserId { get; set; }
    public string? FarmName { get; set; }
    public string? FarmAddress { get; set; }
    public DateTime CreationDate { get; set; }
}
public class FarmDisplayDto
{
    public Guid FarmId { get; set; }
    public Guid UserId { get; set; }
    public string? FarmName { get; set; }
    public string? FarmAddress { get; set; }
    public DateTime CreationDate { get; set; }
}

