
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Application.Mappers;

public static class FarmMapper
{
    public static Farm ToFarm(this FarmCreateDto farmCreateDto)
    {
        return new Farm
        {
            UserId = farmCreateDto.UserId,
            FarmName = farmCreateDto.FarmName,
            FarmAddress = farmCreateDto.FarmAddress,
            CreationDate = farmCreateDto.CreationDate
        };
    }

    public static Farm ToFarm(this FarmUpdateDto farmUpdateDto)
    {
        return new Farm
        {
            UserId = farmUpdateDto.UserId,
            FarmName = farmUpdateDto.FarmName,
            FarmAddress = farmUpdateDto.FarmAddress,
            CreationDate = farmUpdateDto.CreationDate
        };
    }

    public static FarmDisplayDto ToFarmDisplayDto(this Farm farm)
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