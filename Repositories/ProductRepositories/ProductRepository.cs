using Dapper;
using SuperFarm.Models;
using Npgsql;

namespace SuperFarm.Repositories.ProductRepositories;

public class ProductRepository : IProductRepositories
{
    private readonly string _connectionString = "";
    private readonly IConfiguration _config;

    public ProductRepository(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<IEnumerable<Product>> GetAllProductAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Product>("SELECT * FROM farm");
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM product where user_id = @UserId", new { id });
    }

    public async Task<Product> CreateFarmAsync(Product product)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var createdId = await connection.ExecuteScalarAsync<int>("INSERT INTO product (farm_name, farm_address, creation_date) VALUES (@FarmName, @FarmAddress, @CreationDate);select lastval();", product);

        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("UPDATE product SET farm_name = @FarmName, farm_address = @FarmAddress WHERE user_id = @UserId", product);
        return product;
    }

    public async Task DeleteProductAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM product WHERE user_id = @UserId", new { id });
    }

    public Task<Product> CreateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }
}
