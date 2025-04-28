
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.ProductRepositories;

public interface IProductRepositories
{

    Task<IEnumerable<Product>> GetAllProductAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDisplayDto>> QueryProductAsync(string? name, string? catergory);
    Task<Product> CreateProductAsync(ProductCreateDto request);
    Task<Product> UpdateProductByIdAsync(Guid productId, ProductUpdateDto request);
    Task DeleteProductByIdAsync(Guid productId);
}
