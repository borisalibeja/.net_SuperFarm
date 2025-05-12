
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Application.Mappers;

public static class FarmMapper
{
    public static Farm ToFarm(this FarmCreateDto farmCreateDto)
    {
        return new Farm
        {
            FarmName = farmCreateDto.FarmName,
            FarmAbout = farmCreateDto.FarmAbout,
            FarmPhoneNr = farmCreateDto.FarmPhoneNr,
            StreetName = farmCreateDto.StreetName,
            City = farmCreateDto.City,
            Country = farmCreateDto.Country,
            PostCode = farmCreateDto.PostCode,
            BuildingNr = farmCreateDto.BuildingNr,
            ProfileImgUrl = farmCreateDto.ProfileImgUrl,
            CoverImgUrl = farmCreateDto.CoverImgUrl
        };
    }

    public static Farm ToFarm(this FarmUpdateDto farmUpdateDto)
    {
        return new Farm
        {
            FarmName = farmUpdateDto.FarmName,
            FarmAbout = farmUpdateDto.FarmAbout,
            FarmPhoneNr = farmUpdateDto.FarmPhoneNr,
            StreetName = farmUpdateDto.StreetName,
            City = farmUpdateDto.City,
            Country = farmUpdateDto.Country,
            PostCode = farmUpdateDto.PostCode,
            BuildingNr = farmUpdateDto.BuildingNr,
            ProfileImgUrl = farmUpdateDto.ProfileImgUrl,
            CoverImgUrl = farmUpdateDto.CoverImgUrl
        };
    }

    public static FarmDisplayDto ToFarmDisplayDto(this Farm farm)
    {
        return new FarmDisplayDto
        {
            FarmId = farm.FarmId,
            UserId = farm.UserId,
            FarmName = farm.FarmName,
            FarmAbout = farm.FarmAbout,
            FarmPhoneNr = farm.FarmPhoneNr,
            StreetName = farm.StreetName,
            City = farm.City,
            Country = farm.Country,
            PostCode = farm.PostCode,
            BuildingNr = farm.BuildingNr,
            ProfileImgUrl = farm.ProfileImgUrl,
            CoverImgUrl = farm.CoverImgUrl,
            FarmCreatedAt = farm.FarmCreatedAt,
            FarmUpdatedAt = farm.FarmUpdatedAt
        };
    }
}