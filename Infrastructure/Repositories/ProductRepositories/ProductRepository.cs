using System.Data;
using Dapper;
using SuperFarm.Domain.Entities;

namespace SuperFarm.Infrastructure.Repositories.ProductRepositories;

public class ProductRepository(IDbConnection dbConnection) : IProductRepositories
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<IEnumerable<Product>> GetAllProductAsync()
    {
        return await _dbConnection.QueryAsync<Product>("SELECT * FROM farm");
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM product where user_id = @UserId", new { id });
    }

    public async Task<Product> CreateFarmAsync(Product product)
    {
        var createdId = await _dbConnection.ExecuteScalarAsync<int>("INSERT INTO product (farm_name, farm_address, creation_date) VALUES (@FarmName, @FarmAddress, @CreationDate);select lastval();", product);

        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        await _dbConnection.ExecuteAsync("UPDATE product SET farm_name = @FarmName, farm_address = @FarmAddress WHERE user_id = @UserId", product);
        return product;
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await _dbConnection.ExecuteAsync("DELETE FROM product WHERE user_id = @UserId", new { id });
    }

    public Task<Product> CreateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }
}
