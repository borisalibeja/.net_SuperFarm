
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using SuperFarm.Application.DTOs;
using SuperFarm.Domain.Entities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SuperFarm.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public AuthService(IConfiguration configuration, IDbConnection dbConnection)
        {
            _configuration = configuration;
            _dbConnection = dbConnection;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserLoginDto request)
        {

            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE user_name = @Username",
                new { Username = request.Username });

            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        public async Task<User?> RegisterAsync(UserCreateDto request)
        {


            var existingUser = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE user_name = @Username",
                new { Username = request.Username });

            if (existingUser != null)
            {
                return null;
            }

            var user = new User
            {
                Id = Guid.NewGuid(), // Generate a new Guid for the user ID
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                Email = request.Email,
                PhoneNr = request.PhoneNr,
                Address = request.Address,
                Role = request.Role
            };

            user.Password = new PasswordHasher<User>().HashPassword(user, request.Password);

            var sql = "INSERT INTO Users (user_id, user_name, password, first_name, last_name, age, email, phone_nr, address, role) " +
                      "VALUES (@Id, @Username, @Password, @FirstName, @LastName, @Age, @Email, @PhoneNr, @Address, @Role)";

            await _dbConnection.ExecuteAsync(sql, new
            {
                user.Id,
                user.Username,
                user.Password,
                user.FirstName,
                user.LastName,
                user.Age,
                user.Email,
                user.PhoneNr,
                user.Address,
                user.Role
            });

            return user;
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Secret")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User? user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user ?? throw new ArgumentNullException(nameof(user))),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }


        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.Id, request.RefreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {


            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE user_id = @UserId AND RefreshToken = @RefreshToken AND RefreshTokenExpiryTime > @CurrentTime",
                new { UserId = userId, RefreshToken = refreshToken, CurrentTime = DateTime.UtcNow });

            return user;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {


            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            var sql = "UPDATE Users SET RefreshToken = @RefreshToken, RefreshTokenExpiryTime = @RefreshTokenExpiryTime WHERE user_id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { user.RefreshToken, user.RefreshTokenExpiryTime, user.Id });

            return refreshToken;
        }

    }
}
