using Dapper;
using SuperFarm.Models;
using Npgsql;

namespace SuperFarm.Repositories.FarmRepositories;

public class FarmRepository : IFarmRepositories
{
    private readonly string _connectionString = "";
    private readonly IConfiguration _config;

    public FarmRepository(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<IEnumerable<Farm>> GetAllFarmAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Farm>("SELECT * FROM farms");
    }

    public async Task<Farm?> GetFarmByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Farm>("SELECT * FROM farms where user_id = @UserId", new { id });
    }

    public async Task<Farm> CreateFarmAsync(Farm farm)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = "INSERT INTO farms (user_id, farm_name, farm_address, creation_date) VALUES (@user_id, @farm_name, @farm_address, @creation_date) RETURNING user_id";
        var createdId = await connection.ExecuteScalarAsync<int>(sql, farm);
        farm.user_id = createdId;
        return farm;
    }

    public async Task<Farm> UpdateFarmAsync(Farm farm)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("UPDATE farms SET farm_name = @FarmName, farm_address = @FarmAddress WHERE user_id = @UserId", farm);
        return farm;
    }

    public async Task DeleteFarmAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM farms WHERE user_id = @UserId", new { id });
    }
}
