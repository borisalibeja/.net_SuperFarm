using System.Data;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;
using SuperFarm.Services;

namespace SuperFarm.Infrastructure.Repositories.FarmRepositories;

public class FarmRepository(IDbConnection dbConnection, UserContextService userContextService) : IFarmRepositories
{
    private readonly IDbConnection _dbConnection = dbConnection;
    private readonly UserContextService _userContextService = userContextService;

    private const string FarmQuery = @"
    SELECT 
        farm_id as FarmId, 
        user_id as UserId, 
        farm_name as FarmName,
        farm_about as FarmAbout, 
        country_code as CountryCode, 
        farm_phone_nr as FarmPhoneNr,
        farm_email as FarmEmail, 
        profile_image_url as ProfileImageUrl, 
        cover_image_url as CoverImageUrl,
        street_name as StreetName, 
        postcode as PostCode, 
        city as City, 
        country as Country,
        county as County, 
        building_nr as BuildingNr, 
        farm_created_at as FarmCreatedAt, 
        farm_updated_at as FarmUpdatedAt 
    FROM farms";
    public async Task<IEnumerable<Farm>> GetAllFarmAsync()
    {
        var farms = await _dbConnection.QueryAsync<Farm>(FarmQuery);
        return farms;

    }

    public async Task<Farm?> GetFarmByIdAsync(Guid FarmId)
    {
        var sql = $"{FarmQuery} WHERE farm_id = @FarmId";
        var farm = await _dbConnection.QueryFirstOrDefaultAsync<Farm>(sql, new { FarmId });
        return farm;
    }
    public async Task<Farm> GetMyFarmAsync()
    {
        var userId = _userContextService.GetUserId();
        var sql = $"{FarmQuery} WHERE user_id = @userId";
        return (await _dbConnection.QueryFirstOrDefaultAsync<Farm>(sql, new { UserId = userId }).ConfigureAwait(false))!;
    }

    public async Task DeleteMyFarmAsync()
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        if (userId == Guid.Empty || userRole != "Farmer")
        {
            throw new UnauthorizedAccessException("You are Unauthorized");
        }

        var deleteFarmSql = "Delete from farms where user_id = @userId";
        await _dbConnection.ExecuteAsync(deleteFarmSql, new { userId }).ConfigureAwait(false);

        var updateRoleSql = "UPDATE users SET role = 'Customer' WHERE user_id = @userId";
        await _dbConnection.ExecuteAsync(updateRoleSql, new { userId });
    }
    public async Task<Farm> CreateFarmAsync(FarmCreateDto request)
    {
        // Extract user information from the HTTP context
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();

        // Check if the user's role is "Customer"
        if (userRole != "Customer")
        {
            throw new UnauthorizedAccessException($"Only users with the role 'Customer' can create a farm. User Role: {userRole}");
        }

        var existingFarm = GetMyFarmAsync();

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

    public async Task<Farm> UpdateFarmAsync(FarmUpdateDto farmUpdate, FarmDisplayDto farmDisplay, Guid? FarmId)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        if (userRole == "Farmer")
        {
            // Scenario 1: User is a Farmer
            var farm = await GetMyFarmAsync();
            if (farm == null)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this farm.");
            }
            var sql = "UPDATE farms SET farm_name = @FarmName, farm_address = @FarmAddress WHERE user_id = @UserId";
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

        var updatedFarmSql = $"{FarmQuery} WHERE farm_id = @FarmId OR user_id = @UserId";
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
            var farm = await GetMyFarmAsync();
            if (farm == null)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this farm.");
            }
            var sql = "DELETE FROM farms WHERE farm_id = @UserId";
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
        var sql = $"{FarmQuery} WHERE farm_name LIKE @Name";
        var farms = await _dbConnection.QueryAsync<FarmDisplayDto>(sql, new { Name = $"%{farmName}%" });
        return farms;
    }
}
