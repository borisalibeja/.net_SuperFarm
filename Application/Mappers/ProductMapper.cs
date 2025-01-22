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
                ProductId = productCreateDto.ProductId,
                FarmId = productCreateDto.FarmId,
                ProductName = productCreateDto.ProductName,
                ProductPrice = productCreateDto.ProductPrice,
                ProductCategory = productCreateDto.ProductCategory,
            };
        }

        public static Product ToProduct(this ProductUpdateDto productUpdateDto)
        {
            return new Product
            {
                ProductId = productUpdateDto.ProductId,
                FarmId = productUpdateDto.FarmId,
                ProductName = productUpdateDto.ProductName,
                ProductPrice = productUpdateDto.ProductPrice,
                ProductCategory = productUpdateDto.ProductCategory,
            };
        }

        public static ProductDisplayDto ToProductDisplayDto(this Product product)
        {
            return new ProductDisplayDto
            {
                ProductId = product.ProductId,
                FarmId = product.FarmId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductCategory = product.ProductCategory,
            };
        }
    }
}