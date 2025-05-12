using System.Data;
using Dapper;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;
using SuperFarm.Domain.Enums;
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
        return await _dbConnection.QueryFirstOrDefaultAsync<Product>("SELECT product_id as ProductId, farm_id as FarmId," +
        " product_name as ProductName," +
        " product_price as ProductPrice, product_category as ProductCategory FROM products where product_id = @id", new { id });
    }

    public async Task<IEnumerable<ProductDisplayDto>> QueryProductAsync(string? name, string? category)
    {
        var sql = "SELECT product_id as ProductId, farm_id as FarmId, product_name as ProductName, " +
                  "product_price as ProductPrice, product_category as ProductCategory FROM products WHERE 1=1";

        // Add filters dynamically
        if (!string.IsNullOrEmpty(name))
        {
            sql += " AND product_name LIKE @Name";
        }
        if (!string.IsNullOrEmpty(category))
        {
            sql += " AND product_category = @Category";
        }

        // Query parameters
        var parameters = new
        {
            Name = string.IsNullOrEmpty(name) ? null : $"%{name}%",
            Category = category
        };

        // Execute the query
        var products = await _dbConnection.QueryAsync<ProductDisplayDto>(sql, parameters);

        // Convert the product_category string to enum
        foreach (var product in products)
        {
            product.ProductCategory = Enum.Parse<ProductsCategory>(product.ProductCategory.ToString());
        }

        return products;
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
                ProductCategory = product.ProductCategory.ToString()
            }).ConfigureAwait(false);
        }
        else
        {
            throw new UnauthorizedAccessException("Only users with the role 'Farmer' can create a product.");
        }
        return product;

    }

    public async Task<Product> UpdateProductByIdAsync(Guid productId, ProductUpdateDto request)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        var farmIdOfUserLogged = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(
            "SELECT farm_id FROM farms WHERE user_id = @UserId",
            new { UserId = userId }).ConfigureAwait(false);
        var productIdOfUserLogged = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(
            "SELECT product_id FROM products WHERE farm_id = @FarmId",
            new { FarmId = farmIdOfUserLogged }).ConfigureAwait(false);

        if (userRole == "Farmer")
        {
            if (productIdOfUserLogged != productId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this product.");
            }
            else if (productIdOfUserLogged == productId)
            {
                var sql = "UPDATE products SET product_name = @ProductName, product_price = @ProductPrice," +
                " product_category = @ProductCategory WHERE product_id = @ProductId";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    request.ProductName,
                    request.ProductPrice,
                    request.ProductCategory,
                    ProductId = productId
                }).ConfigureAwait(false);
            }

        }
        else if (userRole == "Admin")
        {

            var sql = "UPDATE products SET product_name = @ProductName, product_price = @ProductPrice," +
            " product_category = @ProductCategory WHERE product_id = @ProductId";
            await _dbConnection.ExecuteAsync(sql, new
            {
                request.ProductName,
                request.ProductPrice,
                request.ProductCategory,
                ProductId = productId
            }).ConfigureAwait(false);

        }
        else
        {
            throw new UnauthorizedAccessException("You do not have permission to update this product.");
        }
        var updatedProduct = await GetProductByIdAsync(productId);
        if (updatedProduct == null)
        {
            throw new InvalidOperationException("Product not found after update.");
        }
        return updatedProduct;
    }

    public async Task DeleteProductByIdAsync(Guid productId)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        var farmIdOfUserLogged = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(
            "SELECT farm_id FROM farms WHERE user_id = @UserId",
            new { UserId = userId }).ConfigureAwait(false);
        var productIdOfUserLogged = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(
            "SELECT product_id FROM products WHERE farm_id = @FarmId",
            new { FarmId = farmIdOfUserLogged }).ConfigureAwait(false);

        if (userRole == "Farmer")
        {
            if (productIdOfUserLogged != productId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete a product that you don not own.");
            }
            else if (productIdOfUserLogged == productId)
            {
                var sql = "DELETE products WHERE product_id = @ProductId";
                await _dbConnection.ExecuteAsync(sql, new { ProductId = productId }).ConfigureAwait(false);
            }

        }
        else if (userRole == "Admin")
        {
            var sql = "DELETE products WHERE product_id = @ProductId";
            await _dbConnection.ExecuteAsync(sql, new { ProductId = productId }).ConfigureAwait(false);
        }
        else
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this product.");
        }

    }

}
