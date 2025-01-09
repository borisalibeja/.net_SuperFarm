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

    public static Farm ToFarm(FarmDisplayDto farmDisplayDto)
    {
        return new Farm
        {
            UserId = farmDisplayDto.UserId,
            FarmName = farmDisplayDto.FarmName,
            FarmAddress = farmDisplayDto.FarmAddress,
            CreationDate = farmDisplayDto.CreationDate
        };
    }
}