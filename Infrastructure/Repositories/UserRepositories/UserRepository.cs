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
            var sql = "SELECT * FROM users";
            var users = await connection.QueryAsync<User>(sql);
            foreach (var user in users)
            {
                user.Role = Enum.Parse<Role>(user.Role.ToString());
            }
            return users;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var sql = "SELECT user_id, first_name, last_name, email, password, phone_nr, age, address, role FROM users WHERE user_id = @Id";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
            if (user != null && Enum.TryParse(typeof(Role), user.Role.ToString(), out var role))
            {
                user.Role = (Role)role;
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
                user.Role = Enum.Parse<Role>(user.Role.ToString());
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
            var createdId = await connection.ExecuteScalarAsync<Guid>(sql, new
            {
                user.Username,
                user.Password,
                user.FirstName,
                user.LastName,
                user.Age,
                user.Email,
                user.PhoneNr,
                user.Address,
                Role = user.Role.ToString() // Convert enum to string
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
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM users WHERE id = @Id", new { Id = id });
        }

    }
}