using Dapper;
using SuperFarm.Models;
using Npgsql;

namespace SuperFarm.Repositories;

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
        return await connection.QueryAsync<Farm>("SELECT * FROM farm");
    }

    public async Task<Farm?> GetFarmByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Farm>("SELECT * FROM farm where id = @id", new { id });
    }

    public async Task<Farm> CreateFarmAsync(Farm farm)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var createdId = await connection.ExecuteScalarAsync<int>("INSERT INTO farm (name, location) VALUES (@name, @location);select lastval();", farm);
        farm.Id = createdId;
        return farm;
    }

    public async Task<Farm> UpdateFarmAsync(Farm farm)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("UPDATE farm SET name = @name, location = @location WHERE id = @id", farm);
        return farm;
    }

    public async Task DeleteFarmAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM farm WHERE id = @id", new { id });
    }
}
