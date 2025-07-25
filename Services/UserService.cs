using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Npgsql.Internal;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;
using SuperFarm.Domain.Enums;
using SuperFarm.Services;

namespace SuperFarm.Infrastructure.Repositories.UserRepositories
{
    public class UserRepository(IDbConnection dbConnection, UserContextService userContextService) : IUserRepositories
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly UserContextService _userContextService = userContextService;

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {

            var sql = "SELECT user_id AS UserId, user_name AS Username, password AS Password, first_name AS FirstName," +
            "last_name AS LastName, age AS Age, user_email AS UserEmail, role AS Role FROM Users";
            var users = await _dbConnection.QueryAsync<User>(sql);
            foreach (var user in users)
            {
                user.Role = Enum.Parse<Role>(user.Role.ToString());
            }
            return users;
        }

        public async Task<IEnumerable<UserDisplayDto>> QueryUserByNameAsync(string? name)
        {
            var sql = "SELECT user_id AS UserId, user_name AS Username, password AS Password, first_name AS FirstName," +
            "last_name AS LastName, age AS Age, email AS Email, phone_nr AS PhoneNr, address AS Address, role AS Role " +
            " FROM Users WHERE first_name LIKE @Name OR last_name LIKE @Name";
            var users = await _dbConnection.QueryAsync<UserDisplayDto>(sql, new { Name = $"%{name}%" });
            foreach (var user in users)
            {
                user.Role = Enum.Parse<Role>(user.Role.ToString());
            }
            return users;
        }
        public async Task<User?> GetUserByIdAsync(Guid? id)
        {
            var sql = "SELECT user_id AS UserId, user_name AS Username, password AS Password, " +
            " first_name AS FirstName, last_name AS LastName, age AS Age, user_email AS UserEmail, " +
            "user_phone_nr AS UserPhoneNr, country_code as CountryCode, street_name as StreetName, city as City, country as Country," +
            " county as County, building_nr as BuildingNr, floor_nr as FloorNr, postcode as PostCode," +
            "profile_img_url AS ProfileImgUrl, role AS Role, user_created_at AS UserCreatedAt, " +
            "user_updated_at AS UserUpdatedAt, refresh_token AS RefreshToken, " +
            "refresh_token_expiry_time AS RefreshTokenExpiryTime FROM Users WHERE user_id = @UserId";
            var user = (await _dbConnection.QueryAsync<User>(sql, new { UserId = id }))
                    .Select(u =>
                    {
                        u.FloorNr = u.FloorNr?.ToString(); // Convert FloorNr to string
                        return u;
                    })
                    .FirstOrDefault();
            return user;
        }
        public async Task<User?> GetMyUserInfoAsync()
        {
            var userId = _userContextService.GetUserId();
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Updated user not found.");
            }
            return user;
        }
        public async Task<User> UpdateMyUserAsync(UserUpdateDto request)
        {
            var userId = _userContextService.GetUserId();
            var userRole = _userContextService.GetUserRole();
            var user = await GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new UnauthorizedAccessException("You are not logged in");
            }
            // Dynamically build the SQL query
            var updateFields = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                updateFields.Add("first_name = @FirstName");
                parameters.Add("FirstName", request.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                updateFields.Add("last_name = @LastName");
                parameters.Add("LastName", request.LastName);
            }

            if (!string.IsNullOrWhiteSpace(request.UserEmail))
            {
                updateFields.Add("user_email = @UserEmail");
                parameters.Add("UserEmail", request.UserEmail);
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                string hashedPassword = passwordHasher.HashPassword(user, request.Password);
                updateFields.Add("password = @Password");
                parameters.Add("Password", hashedPassword);
            }

            if (!string.IsNullOrWhiteSpace(request.UserPhoneNr))
            {
                updateFields.Add("user_phone_nr = @UserPhoneNr");
                parameters.Add("UserPhoneNr", request.UserPhoneNr);
            }

            if (!string.IsNullOrWhiteSpace(request.CountryCode))
            {
                updateFields.Add("country_code = @CountryCode");
                parameters.Add("CountryCode", request.CountryCode);
            }

            if (request.Age.HasValue)
            {
                updateFields.Add("age = @Age");
                parameters.Add("Age", request.Age.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.StreetName))
            {
                updateFields.Add("street_name = @StreetName");
                parameters.Add("StreetName", request.StreetName);
            }

            if (!string.IsNullOrWhiteSpace(request.City))
            {
                updateFields.Add("city = @City");
                parameters.Add("City", request.City);
            }

            if (!string.IsNullOrWhiteSpace(request.Country))
            {
                updateFields.Add("country = @Country");
                parameters.Add("Country", request.Country);
            }
            if (!string.IsNullOrWhiteSpace(request.County))
            {
                updateFields.Add("county = @County");
                parameters.Add("County", request.County);
            }
            if (!string.IsNullOrWhiteSpace(request.BuildingNr))
            {
                updateFields.Add("building_nr = @BuildingNr");
                parameters.Add("BuildingNr", request.BuildingNr);
            }

            if (!string.IsNullOrWhiteSpace(request.FloorNr))
            {
                updateFields.Add("floor_nr = @FloorNr");
                parameters.Add("FloorNr", request.FloorNr);
            }
            if (!string.IsNullOrWhiteSpace(request.PostCode))
            {
                updateFields.Add("postcode = @PostCode");
                parameters.Add("PostCode", request.PostCode);
            }

            if (request.Role.ToString() != userRole)
            {
                updateFields.Add("role = @Role::text");
                parameters.Add("Role", request.Role.ToString());
            }

            // If no fields were provided to update, throw an exception
            if (!updateFields.Any())
            {
                throw new InvalidOperationException("No fields were provided to update.");
            }

            // Combine the dynamically built fields into the SQL query
            var updateUserSql = $@"UPDATE users
                 SET {string.Join(", ", updateFields)}
                 WHERE user_id = @UserId";

            parameters.Add("UserId", userId);

            await _dbConnection.ExecuteAsync(updateUserSql, parameters).ConfigureAwait(false);

            var updatedUser = await GetUserByIdAsync(userId);
            if (updatedUser == null)
            {
                throw new InvalidOperationException("Updated user not found.");
            }
            return updatedUser;
        }
        public async Task<User> UpdateUserAsync(UserUpdateDto request, Guid? UserId)
        {
            var userId = _userContextService.GetUserId();
            var userRole = _userContextService.GetUserRole();

            if (userRole == "Customer")
            {
                // Scenario 1: User is a Customer
                var user = await GetUserByIdAsync(userId);
                if (user == null || userId != UserId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to update this user.");
                }
                var sql = @"UPDATE users
                SET first_name = @FirstName, last_name = @Lastname, user_email = @UserEmail, password = @Password, " +
                " user_phone_nr = @UserPhoneNr, age = @Age, street_name = @StreetName, role = @Role::text " +
                "WHERE user_id = @UserId";
                if (string.IsNullOrEmpty(request.Password))
                {
                    throw new ArgumentException("Password cannot be null or empty.");
                }
                var passwordHasher = new PasswordHasher<User>();
                string hashedPassword = passwordHasher.HashPassword(user, request.Password);
                await _dbConnection.ExecuteAsync(sql, new
                {
                    UserId = userId,
                    Password = hashedPassword,
                    request.FirstName,
                    request.LastName,
                    request.Age,
                    request.UserEmail,
                    request.UserPhoneNr,
                    request.StreetName,
                    Role = request.Role.ToString()  // Convert enum to string
                }).ConfigureAwait(false);
            }
            else if (userRole == "Admin")
            {
                // Scenario 2: User is a Admin

                var user = await GetUserByIdAsync(UserId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }
                else
                {
                    var sql = @"UPDATE users
                    SET first_name = @FirstName, last_name = @Lastname, user_email = @UserEmail, password = @Password, phone_nr = @PhoneNr, age = @Age, address = @Address, role = @Role::text
                    WHERE user_id = @UserId";
                    if (string.IsNullOrEmpty(request.Password))
                    {
                        throw new ArgumentException("Password cannot be null or empty.");
                    }
                    var passwordHasher = new PasswordHasher<User>();
                    string hashedPassword = passwordHasher.HashPassword(user, request.Password);
                    await _dbConnection.ExecuteAsync(sql, new
                    {
                        Password = hashedPassword,
                        request.FirstName,
                        request.LastName,
                        request.Age,
                        request.UserEmail,
                        request.UserPhoneNr,
                        request.StreetName,
                        Role = request.Role.ToString()  // Convert enum to string
                    }).ConfigureAwait(false);
                }
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to update this user.");
            }

            var updatedUserSql = "SELECT user_id AS UserId, user_name AS Username, password AS Password, first_name AS FirstName," +
            "last_name AS LastName, age AS Age, email AS Email, phone_nr AS PhoneNr, address AS Address, role AS Role FROM Users WHERE user_id = @UserId";
            var updatedUser = await _dbConnection.QueryFirstOrDefaultAsync<User>(updatedUserSql, new { UserId }).ConfigureAwait(false);
            if (updatedUser == null)
            {
                throw new InvalidOperationException("Updated user not found.");
            }
            return updatedUser;

        }

        public async Task DeleteUserAsync(Guid UserId)
        {
            var userId = _userContextService.GetUserId();
            var userRole = _userContextService.GetUserRole();

            if (userRole == "Customer")
            {
                if (userId != UserId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to delete this user.");
                }
                var sql = "DELETE FROM users WHERE user_id = @UserId";
                await _dbConnection.ExecuteAsync(sql, new { UserId = userId }).ConfigureAwait(false);
            }
            else if (userRole == "Admin")
            {
                var user = await GetUserByIdAsync(UserId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }
                var sql = "DELETE FROM users WHERE user_id = @UserId";
                await _dbConnection.ExecuteAsync(sql, new { UserId }).ConfigureAwait(false);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this user.");
            }
        }


        public async Task DeleteMyUserAsync()
        {
            var userId = _userContextService.GetUserId();
            var user = await GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new UnauthorizedAccessException("You are not logged in");
            }
            var sql = "Delete from users where user_id = @UserId";
            await _dbConnection.ExecuteAsync(sql, new { UserId = userId }).ConfigureAwait(false);

        }

    }
}