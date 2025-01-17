using Dapper;
using Npgsql;
using SuperFarm.Domain.Entities;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Infrastructure.Repositories.UserRepositories
{
    public class UserRepository : IUserRepositories
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public UserRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var sql = "SELECT id, name, surname, email, password, phone_nr AS PhoneNr, age, address, role_name AS RoleName FROM users";
            var users = await connection.QueryAsync<User>(sql);
            foreach (var user in users)
            {
                user.RoleName = Enum.Parse<Role>(user.RoleName.ToString());
            }
            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var sql = "SELECT id, name, surname, email, password, phone_nr AS PhoneNr, age, address, role_name AS RoleName FROM users WHERE id = @Id";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
            if (user != null)
            {
                user.RoleName = Enum.Parse<Role>(user.RoleName.ToString());
            }
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var sql = "SELECT id, name, surname, email, password, phone_nr AS PhoneNr, age, address, role_name AS RoleName FROM users WHERE email = @Email";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
            if (user != null)
            {
                user.RoleName = Enum.Parse<Role>(user.RoleName.ToString());
            }
            return user;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var sql = @"
                INSERT INTO users (name, surname, email, password, phone_nr, age, address, role_name) 
                VALUES (@Name, @Surname, @Email, @Password, @PhoneNr, @Age, @Address, @RoleName) 
                RETURNING id";
            var createdId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                user.Name,
                user.Surname,
                user.Email,
                user.Password,
                user.PhoneNr,
                user.Age,
                user.Address,
                RoleName = user.RoleName.ToString() // Convert enum to string
            });
            user.Id = createdId;
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var sql = @"
                UPDATE users 
                SET name = @Name, surname = @Surname, email = @Email, password = @Password, phone_nr = @PhoneNr, age = @Age, address = @Address, role_name = @RoleName::text 
                WHERE id = @Id 
                RETURNING id, name, surname, email, password, phone_nr AS PhoneNr, age, address, role_name AS RoleName";
            var updatedUser = await connection.QueryFirstOrDefaultAsync<User>(sql, new
            {
                user.Id,
                user.Name,
                user.Surname,
                user.Email,
                user.Password,
                user.PhoneNr,
                user.Age,
                user.Address,
                RoleName = user.RoleName.ToString() // Convert enum to string
            }) ?? throw new InvalidOperationException("User update failed.");
            return updatedUser;
        }

        public async Task DeleteUserAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM users WHERE id = @Id", new { Id = id });
        }

    }
}