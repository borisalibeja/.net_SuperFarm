using System;
using SuperFarm.Models;
using SuperFarm.Models.DTOs.FarmDTOs;

namespace SuperFarm.Mappers;

public static class FarmMapper
{
    public static Farm ToFarm(FarmCreateDto farmCreateDto)
    {
        return new Farm
        {
            UserId = farmCreateDto.UserId,
            FarmName = farmCreateDto.FarmName,
            FarmAddress = farmCreateDto.FarmAddress,
            CreationDate = farmCreateDto.CreationDate
        };
    }

    public static Farm ToFarm(FarmUpdateDto farmUpdateDto)
    {
        return new Farm
        {
            UserId = farmUpdateDto.UserId,
            FarmName = farmUpdateDto.FarmName,
            FarmAddress = farmUpdateDto.FarmAddress,
            CreationDate = farmUpdateDto.CreationDate
        };
    }

    public static FarmDisplayDto ToFarmDisplayDto(Farm farm)
    {
        return new FarmDisplayDto
        {
            UserId = farm.UserId,
            FarmName = farm.FarmName ?? string.Empty,
            FarmAddress = farm.FarmAddress ?? string.Empty,
            CreationDate = farm.CreationDate
        };
    }
}