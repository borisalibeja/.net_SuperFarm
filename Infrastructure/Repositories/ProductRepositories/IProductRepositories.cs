
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.ProductRepositories;

public interface IProductRepositories
{

    Task<IEnumerable<Product>> GetAllProductAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product> CreateProductAsync(ProductCreateDto request);
    Task<Product> UpdateProductAsync(ProductUpdateDto request);
    Task DeleteProductAsync(Guid id);
}
