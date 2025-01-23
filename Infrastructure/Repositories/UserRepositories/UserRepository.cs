using System.Data;
using Dapper;
using Npgsql;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Infrastructure.Repositories.UserRepositories
{
    public class UserRepository(IDbConnection dbConnection) : IUserRepositories
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {

            var sql = "SELECT user_id AS Id, user_name AS Username, password AS Password, first_name AS FirstName," +
            "last_name AS LastName, age AS Age, email AS Email, phone_nr AS PhoneNr, address AS Address, role AS Role FROM Users";
            var users = await _dbConnection.QueryAsync<User>(sql);
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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql = "SELECT user_id AS Id, user_name AS Username, password AS Password, first_name AS FirstName, last_name AS LastName," +
                      " age AS Age, email AS Email, phone_nr AS PhoneNr, address AS Address, role AS Role FROM Users WHERE email = @Email";
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
            if (user != null)
            {
                user.Role = Enum.Parse<Role>(user.Role.ToString());
            }
            return user;
        }


        public async Task<User> UpdateUserAsync(User user)
        {
            var sql = @"
                UPDATE users 
                SET first_name = @FirstName, last_name = @Lastname, email = @Email, password = @Password, phone_nr = @PhoneNr, age = @Age, address = @Address, role = @Role::text 
                WHERE user_id = @Id 
                RETURNING user_id AS Id, user_name AS Username, password AS Password, first_name AS FirstName, last_name AS LastName," +
                      " age AS Age, email AS Email, phone_nr AS PhoneNr, address AS Address, role AS Role";
            var updatedUser = await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new
            {
                user.Username,
                user.Password,
                user.FirstName,
                user.LastName,
                user.Age,
                user.Email,
                user.PhoneNr,
                user.Address,
                Role = user.Role.ToString()  // Convert enum to string
            }) ?? throw new InvalidOperationException("User update failed.");
            return updatedUser;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _dbConnection.ExecuteAsync("DELETE FROM users WHERE user_id = @Id", new { Id = id });
        }

    }
}