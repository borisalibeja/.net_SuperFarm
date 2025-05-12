using System;

namespace SuperFarm.Application.DTOs;

public class FarmCreateDto
{
    public string? FarmName { get; set; }
    public string? FarmAbout { get; set; }
    public string? FarmPhoneNr { get; set; }
    public string? StreetName { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? County { get; set; }
    public string? BuildingNr { get; set; }
    public string? PostCode { get; set; }
    public string? ProfileImgUrl { get; set; }
    public string? CoverImgUrl { get; set; }

}
public class FarmUpdateDto
{
    public string? FarmName { get; set; }
    public string? FarmAbout { get; set; }
    public string? FarmPhoneNr { get; set; }
    public string? StreetName { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? County { get; set; }
    public string? BuildingNr { get; set; }
    public string? PostCode { get; set; }
    public string? ProfileImgUrl { get; set; }
    public string? CoverImgUrl { get; set; }
}
public class FarmDisplayDto
{
    public Guid FarmId { get; set; }
    public Guid UserId { get; set; }
    public string? FarmName { get; set; }
    public string? FarmAbout { get; set; }
    public string? FarmPhoneNr { get; set; }
    public string? StreetName { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? County { get; set; }
    public string? BuildingNr { get; set; }
    public string? PostCode { get; set; }
    public string? ProfileImgUrl { get; set; }
    public string? CoverImgUrl { get; set; }
    public DateTime? FarmCreatedAt { get; set; }
    public DateTime? FarmUpdatedAt { get; set; }
}

