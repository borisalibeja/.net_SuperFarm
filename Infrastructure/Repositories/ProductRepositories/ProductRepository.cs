using System.Data;
using Dapper;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;
using SuperFarm.Services;

namespace SuperFarm.Infrastructure.Repositories.ProductRepositories;

public class ProductRepository(IDbConnection dbConnection, UserContextService userContextService) : IProductRepositories
{
    private readonly IDbConnection _dbConnection = dbConnection;
    private readonly UserContextService _userContextService = userContextService;

    public async Task<IEnumerable<Product>> GetAllProductAsync()
    {
        return await _dbConnection.QueryAsync<Product>("SELECT product_id as ProductId, farm_id as FarmId, product_name as ProductName," +
        " product_price as ProductPrice, product_category as ProductCategory FROM products");
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Product>("SELECT product_id as ProductId, farm_id as FarmId, product_name as ProductName," +
        " product_price as ProductPrice, product_category as ProductCategory FROM products where product_id = @id", new { id });
    }

    public async Task<Product> CreateProductAsync(ProductCreateDto request)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        var farmId = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(
            "SELECT farm_id FROM farms WHERE user_id = @UserId",
            new { UserId = userId }).ConfigureAwait(false);
        var product = new Product
        {
            ProductId = Guid.NewGuid(), // Generate a new Guid for the product ID
            FarmId = farmId,
            ProductName = request.ProductName,
            ProductPrice = request.ProductPrice,
            ProductCategory = request.ProductCategory
        };

        // Check if the user's role is "Farmer"
        if (userRole == "Farmer")
        {
            var newProduct = "INSERT INTO products (product_id, farm_id, product_name, product_price, product_category) " +
                    "VALUES (@ProductId, @FarmId, @ProductName, @ProductPrice, @ProductCategory)";
            await _dbConnection.ExecuteAsync(newProduct, new
            {
                product.ProductId,
                product.FarmId,
                product.ProductName,
                product.ProductPrice,
                product.ProductCategory
            }).ConfigureAwait(false);
        }
        else
        {
            throw new UnauthorizedAccessException("Only users with the role 'Farmer' can create a product.");
        }
        return product;

    }

    public async Task<Product> UpdateProductAsync(ProductUpdateDto request)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();

        if (userRole != "Farmer")
        {
            throw new UnauthorizedAccessException("Only users with the role 'Farmer' can update a product.");
        }

        var product = await GetProductByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        if (product.FarmId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this product.");
        }

        var sql = @"UPDATE products
                    SET product_name = @ProductName, product_price = @ProductPrice, product_category = @ProductCategory
                    WHERE product_id = @ProductId";

        await _dbConnection.ExecuteAsync(sql, new
        {
            request.ProductId,
            request.ProductName,
            request.ProductPrice,
            request.ProductCategory
        });

        return product;
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await _dbConnection.ExecuteAsync("DELETE FROM products WHERE product_id = @ProductId", new { ProductId = id });
    }

}
