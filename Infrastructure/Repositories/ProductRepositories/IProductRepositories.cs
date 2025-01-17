
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.ProductRepositories;

public interface IProductRepositories
{

    Task<IEnumerable<Product>> GetAllProductAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
}
