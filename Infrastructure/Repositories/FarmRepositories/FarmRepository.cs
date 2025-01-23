using System.Data;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;
using SuperFarm.Services;

namespace SuperFarm.Infrastructure.Repositories.FarmRepositories;

public class FarmRepository(IDbConnection dbConnection, UserContextService userContextService) : IFarmRepositories
{
    private readonly IDbConnection _dbConnection = dbConnection;
    private readonly UserContextService _userContextService = userContextService;

    public async Task<IEnumerable<Farm>> GetAllFarmAsync()
    {
        return await _dbConnection.QueryAsync<Farm>("SELECT * FROM farms");
    }

    public async Task<Farm?> GetFarmByIdAsync(Guid id)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Farm>("SELECT * FROM farms where user_id = @UserId", new { id });
    }

    public async Task<Farm> CreateFarmAsync(FarmCreateDto request)
    {
        // Extract user information from the HTTP context
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();

        // Check if the user's role is "Customer"
        if (userRole != "Customer")
        {
            throw new UnauthorizedAccessException("Only users with the role 'Customer' can create a farm."); // You may throw an exception or return an appropriate response
        }

        var existingFarm = await _dbConnection.QueryFirstOrDefaultAsync<Farm>(
            "SELECT * FROM farms WHERE user_id = @UserId",
            new { UserId = userId }).ConfigureAwait(false);

        if (existingFarm != null)
        {
            throw new InvalidOperationException("A farm already exists for this user.");
        }

        var farm = new Farm
        {
            FarmId = Guid.NewGuid(), // Generate a new Guid for the farm ID
            FarmName = request.FarmName,
            FarmAddress = request.FarmAddress,
            UserId = userId // Assign the farm to the current user
        };

        var sql = "INSERT INTO farms (farm_id, farm_name, farm_address, user_id) " +
                  "VALUES (@FarmId, @FarmName, @FarmAddress, @UserId)";

        await _dbConnection.ExecuteAsync(sql, new
        {
            farm.FarmId,
            farm.FarmName,
            farm.FarmAddress,
            farm.UserId
        }).ConfigureAwait(false);

        // Update the user's role to "Farmer"
        var updateRoleSql = "UPDATE users SET role = @Role WHERE user_id = @UserId";
        await _dbConnection.ExecuteAsync(updateRoleSql, new
        {
            Role = "Farmer",
            UserId = userId
        }).ConfigureAwait(false);

        return farm;

    }

    public async Task<Farm> UpdateFarmAsync(FarmUpdateDto request)
    {
        // Extract user information from the HTTP context
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();

        // Check if the user's role is "Farmer"
        if (userRole == "Farmer")
        {
            throw new UnauthorizedAccessException("Only users with the role 'Farmer' can update a farm.");
        }

        // Check if the farm belongs to the current user
        var existingFarm = await _dbConnection.QueryFirstOrDefaultAsync<Farm>(
            "SELECT * FROM farms WHERE farm_id = @FarmId AND user_id = @UserId",
            new { FarmId = request.FarmId, UserId = userId }).ConfigureAwait(false);

        if (existingFarm == null)
        {
            throw new InvalidOperationException("Farm not found or does not belong to the user."); // Farm not found or does not belong to the user
        }

        // Update the farm details
        var sql = "UPDATE farms SET farm_name = @FarmName, farm_address = @FarmAddress WHERE farm_id = @FarmId";
        await _dbConnection.ExecuteAsync(sql, new
        {
            request.FarmId,
            request.FarmName,
            request.FarmAddress
        }).ConfigureAwait(false);

        // Return the updated farm object
        var updatedFarm = await _dbConnection.QueryFirstOrDefaultAsync<Farm>(
            "SELECT * FROM farms WHERE farm_id = @FarmId",
            new { request.FarmId }).ConfigureAwait(false);

        return updatedFarm ?? throw new InvalidOperationException("Farm update failed.");

    }

    public async Task DeleteFarmAsync(Guid id)
    {
        await _dbConnection.ExecuteAsync("DELETE FROM farms WHERE user_id = @UserId", new { id });
    }
}
