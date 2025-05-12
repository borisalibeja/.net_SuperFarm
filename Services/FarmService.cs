using System.Data;
using Dapper;
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
        return await _dbConnection.QueryAsync<Farm>("SELECT farm_id as FarmId, user_id as UserId, farm_name as FarmName," +
        " farm_address as FarmAddress, creation_date as CreationDate FROM farms");

    }

    public async Task<Farm?> GetFarmByIdAsync(Guid FarmId)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Farm>("SELECT farm_id as FarmId, user_id as UserId, farm_name as FarmName," +
        " farm_address as FarmAddress, creation_date as CreationDate FROM farms WHERE farm_id = @FarmId", new { FarmId });
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
            UserId = userId // Assign the farm to the current user
        };

        var sql = "INSERT INTO farms (farm_id, farm_name, farm_address, user_id) " +
                  "VALUES (@FarmId, @FarmName, @FarmAddress, @UserId)";

        await _dbConnection.ExecuteAsync(sql, new
        {
            farm.FarmId,
            farm.FarmName,
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

    public async Task<Farm> GetFarmByUserIdAsync()
    {
        var userId = _userContextService.GetUserId();
        var sql = "SELECT * FROM farms WHERE user_id = @userId";
        return (await _dbConnection.QueryFirstOrDefaultAsync<Farm>(sql, new { UserId = userId }).ConfigureAwait(false))!;
    }
    public async Task<Farm> UpdateFarmAsync(FarmUpdateDto farmUpdate, FarmDisplayDto farmDisplay, Guid? FarmId)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        if (userRole == "Farmer")
        {
            // Scenario 1: User is a Farmer
            var farm = await GetFarmByUserIdAsync();
            if (farm == null)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this farm.");
            }
            var sql = "UPDATE farms SET farm_name = @FarmName, farm_address = @FarmAddress WHERE farm_id = (SELECT farm_id FROM farms WHERE user_id = @UserId)";
            await _dbConnection.ExecuteAsync(sql, new
            {
                UserId = userId, // Use the farm ID from the retrieved farm object
                farmUpdate.FarmName,
            }).ConfigureAwait(false);
        }
        else if (userRole == "Admin")
        {
            // Scenario 2: User is not a Farmer
            if (farmDisplay.FarmId == Guid.Empty)
            {
                throw new ArgumentException("Farm ID is required.");
            }
            var farm = await GetFarmByIdAsync(farmDisplay.FarmId);
            if (farm == null)
            {
                throw new InvalidOperationException("Farm not found.");
            }
            else
            {
                var sql = "UPDATE farms SET farm_name = @FarmName, farm_address = @FarmAddress WHERE farm_id = @FarmId";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    farmDisplay.FarmId,
                    farmUpdate.FarmName,
                }).ConfigureAwait(false);
            }
        }

        var updatedFarmSql = "SELECT farm_id as FarmId, user_id as UserId, farm_name as FarmName, farm_address as FarmAddress, creation_date as CreationDate" +
        " FROM farms WHERE farm_id = @FarmId OR user_id = @UserId";
        var updatedFarm = await _dbConnection.QueryFirstOrDefaultAsync<Farm>(updatedFarmSql, new { farmDisplay.FarmId, UserId = userId }).ConfigureAwait(false);
        if (updatedFarm == null)
        {
            throw new InvalidOperationException("Updated farm not found.");
        }
        return updatedFarm;


    }

    public async Task DeleteFarmAsync(Guid? FarmId)

    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();

        if (userRole == "Farmer")
        {
            var farm = await GetFarmByUserIdAsync();
            if (farm == null)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this farm.");
            }
            var sql = "DELETE FROM farms WHERE farm_id = (SELECT farm_id FROM farms WHERE user_id = @UserId)";
            await _dbConnection.ExecuteAsync(sql, new { UserId = userId }).ConfigureAwait(false);
            var updateRoleSql = "UPDATE users SET role = @Role WHERE user_id = @UserId";
            await _dbConnection.ExecuteAsync(updateRoleSql, new
            {
                Role = "Customer",
                UserId = userId
            });
        }
        else
        {
            if (FarmId == null || FarmId == Guid.Empty)
            {
                throw new ArgumentException("Farm ID is required.");
            }
            var farm = await GetFarmByIdAsync(FarmId.Value);
            if (farm == null)
            {
                throw new InvalidOperationException("Farm not found.");
            }
            var updateRoleSql = "UPDATE users SET role = @Role WHERE user_id = (SELECT user_id FROM farms WHERE farm_id = @farmId)";
            await _dbConnection.ExecuteAsync(updateRoleSql, new
            {
                Role = "Customer",
                farmId = FarmId
            }).ConfigureAwait(false);

            var sql = "DELETE FROM farms WHERE farm_id = @farmId";
            await _dbConnection.ExecuteAsync(sql, new { farmId = FarmId }).ConfigureAwait(false);

        }
    }

    public async Task<IEnumerable<FarmDisplayDto>> QueryFarmByNameAsync(string? farmName)
    {
        var sql = "SELECT farm_id as FarmId, user_id as UserId, farm_name as FarmName," +
        " farm_address as FarmAddress, creation_date as CreationDate FROM farms WHERE farm_name LIKE @Name";
        var farms = await _dbConnection.QueryAsync<FarmDisplayDto>(sql, new { Name = $"%{farmName}%" });
        return farms;
    }
}
