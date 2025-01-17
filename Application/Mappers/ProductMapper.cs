using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Application.Mappers
{
    public static class ProductMapper
    {
        public static Product ToProduct(this ProductCreateDto productCreateDto)
        {
            return new Product
            {
                ProductName = productCreateDto.ProductName,
                ProductPrice = productCreateDto.ProductPrice,
                ProductCategory = productCreateDto.ProductCategory,
                FarmId = productCreateDto.FarmId
            };
        }

        public static Product ToProduct(this ProductUpdateDto productUpdateDto)
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

        public static ProductDisplayDto ToProductDisplayDto(this Product product)
        {
            return new ProductDisplayDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductCategory = product.ProductCategory,
                FarmId = product.FarmId
            };
        }
    }
}