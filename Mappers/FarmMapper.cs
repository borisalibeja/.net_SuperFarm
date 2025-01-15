using System;
using SuperFarm.Models;
using SuperFarm.Models.DTOs.FarmDTOs;

namespace SuperFarm.Mappers;

public static class FarmMapper
{
    public static Farm ToFarm(this FarmCreateDto farmCreateDto)
    {
        return new Farm
        {
            user_id = farmCreateDto.user_id,
            farm_name = farmCreateDto.farm_name,
            farm_address = farmCreateDto.farm_address,
            creation_date = farmCreateDto.creation_date
        };
    }

    public static Farm ToFarm(this FarmUpdateDto farmUpdateDto)
    {
        return new Farm
        {
            user_id = farmUpdateDto.UserId,
            farm_name = farmUpdateDto.FarmName,
            farm_address = farmUpdateDto.FarmAddress,
            creation_date = farmUpdateDto.CreationDate
        };
    }

    public static FarmDisplayDto ToFarmDisplayDto(this Farm farm)
    {
        return new FarmDisplayDto
        {
            UserId = farm.user_id,
            FarmName = farm.farm_name ?? string.Empty,
            FarmAddress = farm.farm_address ?? string.Empty,
            CreationDate = farm.creation_date
        };
    }
}