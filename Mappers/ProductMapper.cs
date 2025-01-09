using SuperFarm.Models;
using SuperFarm.Models.DTOs.ProductDTOs;

namespace SuperFarm.Mappers;


public static class ProductMapper
{
    public static Product ToProduct(ProductDisplayDto productDisplayDto)
    {
        return new Product
        {
            Id = productDisplayDto.Id,
            ProductName = productDisplayDto.ProductName,
            ProductPrice = productDisplayDto.ProductPrice,
            ProductCategory = productDisplayDto.ProductCategory,
            FarmId = productDisplayDto.FarmId
        };
    }

    public static Product ToProduct(ProductCreateDto productCreateDto)
    {
        return new Product
        {
            Id = productCreateDto.Id,
            ProductName = productCreateDto.ProductName,
            ProductPrice = productCreateDto.ProductPrice,
            ProductCategory = productCreateDto.ProductCategory,
            FarmId = productCreateDto.FarmId
        };
    }

    public static Product ToProduct(ProductUpdateDto productUpdateDto)
    {
        return new Product
        {
            Id = productUpdateDto.Id,
            ProductName = productUpdateDto.ProductName,
            ProductPrice = productUpdateDto.ProductPrice,
            ProductCategory = productUpdateDto.ProductCategory,
            FarmId = productUpdateDto.FarmId
        };
    }
}

