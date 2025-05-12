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
                ProductCategory = productCreateDto.ProductCategory,
                ProductDescription = productCreateDto.ProductDescription,
                ProductPrice = productCreateDto.ProductPrice,
                Currency = productCreateDto.Currency,
                StockUnit = productCreateDto.StockUnit,
                StockWeight = productCreateDto.StockWeight,
                ImgUrl1 = productCreateDto.ImgUrl1,
                ImgUrl2 = productCreateDto.ImgUrl2,
                ImgUrl3 = productCreateDto.ImgUrl3,
                ImgUrl4 = productCreateDto.ImgUrl4,
                ImgUrl5 = productCreateDto.ImgUrl5
            };
        }

        public static Product ToProduct(this ProductUpdateDto productUpdateDto)
        {
            return new Product
            {
                ProductName = productUpdateDto.ProductName,
                ProductCategory = productUpdateDto.ProductCategory,
                ProductDescription = productUpdateDto.ProductDescription,
                ProductPrice = productUpdateDto.ProductPrice,
                Currency = productUpdateDto.Currency,
                StockUnit = productUpdateDto.StockUnit,
                StockWeight = productUpdateDto.StockWeight,
                ImgUrl1 = productUpdateDto.ImgUrl1,
                ImgUrl2 = productUpdateDto.ImgUrl2,
                ImgUrl3 = productUpdateDto.ImgUrl3,
                ImgUrl4 = productUpdateDto.ImgUrl4,
                ImgUrl5 = productUpdateDto.ImgUrl5
            };
        }

        public static ProductDisplayDto ToProductDisplayDto(this Product product)
        {
            return new ProductDisplayDto
            {
                ProductId = product.ProductId,
                FarmId = product.FarmId,
                ProductName = product.ProductName,
                ProductCategory = product.ProductCategory,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                Currency = product.Currency,
                StockUnit = product.StockUnit,
                StockWeight = product.StockWeight,
                ImgUrl1 = product.ImgUrl1,
                ImgUrl2 = product.ImgUrl2,
                ImgUrl3 = product.ImgUrl3,
                ImgUrl4 = product.ImgUrl4,
                ImgUrl5 = product.ImgUrl5,
                ProductCreatedAt = product.ProductCreatedAt,
                ProductUpdatedAt = product.ProductUpdatedAt
            };
        }
    }
}