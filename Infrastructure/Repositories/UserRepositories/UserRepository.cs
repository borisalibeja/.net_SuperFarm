using System.Data;
using Dapper;
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
            "last_name AS LastName, age AS Age, email AS Email, phone_nr AS PhoneNr, address AS Address, role AS Role FROM Users";
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
            "last_name AS LastName, age AS Age, email AS Email, phone_nr AS PhoneNr, address AS Address, role AS Role FROM Users WHERE first_name LIKE @Name OR last_name LIKE @Name";
            var users = await _dbConnection.QueryAsync<UserDisplayDto>(sql, new { Name = $"%{name}%" });
            foreach (var user in users)
            {
                user.Role = Enum.Parse<Role>(user.Role.ToString());
            }
            return users;
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var sql = "SELECT user_id AS Id, user_name AS Username, password AS Password, first_name AS FirstName, last_name AS LastName," +
                      " age AS Age, email AS Email, phone_nr AS PhoneNr, address AS Address, role AS Role FROM Users WHERE user_id = @Id";
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
            if (user != null && Enum.TryParse(typeof(Role), user.Role.ToString(), out var role))
            {
                user.Role = (Role)role;
            }
            return user;
        }
        public async Task<User> UpdateUserAsync(UserUpdateDto request, Guid? UserId)
        {
            var userId = _userContextService.GetUserId();
            var userRole = _userContextService.GetUserRole();

            if (userRole == "Customer")
            {
                // Scenario 1: User is a Farmer
                var user = await GetUserByIdAsync(userId);
                if (user == null || userId == UserId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to update this user.");
                }
                var sql = @"UPDATE users
                SET first_name = @FirstName, last_name = @Lastname, email = @Email, password = @Password, phone_nr = @PhoneNr, age = @Age, address = @Address, role = @Role::text
                WHERE user_id = @UserId";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    UserId = userId, // Use the farm ID from the retrieved farm object
                    request.Username,
                    request.Password,
                    request.FirstName,
                    request.LastName,
                    request.Age,
                    request.Email,
                    request.PhoneNr,
                    request.Address,
                    Role = request.Role.ToString()  // Convert enum to string
                }).ConfigureAwait(false);
            }
            else if (userRole == "Admin")
            {
                // Scenario 2: User is not a Customer
                if (request.UserId == Guid.Empty)
                {
                    throw new ArgumentException("User ID is required.");
                }
                var user = await GetUserByIdAsync(request.UserId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }
                else
                {
                    var sql = @"UPDATE users
                    SET first_name = @FirstName, last_name = @Lastname, email = @Email, password = @Password, phone_nr = @PhoneNr, age = @Age, address = @Address, role = @Role::text
                    WHERE user_id = @UserId";
                    await _dbConnection.ExecuteAsync(sql, new
                    {
                        request.UserId, // Use the farm ID from the retrieved farm object
                        request.Username,
                        request.Password,
                        request.FirstName,
                        request.LastName,
                        request.Age,
                        request.Email,
                        request.PhoneNr,
                        request.Address,
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
            var updatedUser = await _dbConnection.QueryFirstOrDefaultAsync<User>(updatedUserSql, new { request.UserId }).ConfigureAwait(false);
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

    }
}