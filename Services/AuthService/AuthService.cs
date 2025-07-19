
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
            var sql = "SELECT user_id as UserId, user_name AS Username, role as Role, password AS Password FROM Users WHERE user_name = @UserName";
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = request.Username }).ConfigureAwait(false);
            if (user == null)
            {
                Console.WriteLine("User not found");
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password)
                == PasswordVerificationResult.Failed)
            {
                Console.WriteLine("Password verification failed");
                return null;
            }

            Console.WriteLine($"User found: {user.Username}, Id: {user.UserId}, Role: {user.Role}");
            return await CreateTokenResponse(user).ConfigureAwait(false);
        }

        public async Task<User?> RegisterAsync(UserCreateDto request)
        {


            var existingUser = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE user_name = @UserName",
                new { UserName = request.Username });

            if (existingUser != null)
            {
                return null;
            }

            var user = new User
            {
                UserId = Guid.NewGuid(), // Generate a new Guid for the user ID
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                UserEmail = request.UserEmail,
                UserPhoneNr = request.UserPhoneNr,
                StreetName = request.StreetName,
                Role = Domain.Enums.Role.Customer
            };
            if (string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Password cannot be null or empty.");
            }
            user.Password = new PasswordHasher<User>().HashPassword(user, request.Password);

            var sql = "INSERT INTO Users (user_id, user_name, password, first_name, last_name, age, user_email, user_phone_nr, role) " +
                      "VALUES (@UserId, @Username, @Password, @FirstName, @LastName, @Age, @UserEmail, @UserPhoneNr, @Role)";

            await _dbConnection.ExecuteAsync(sql, new
            {
                user.UserId,
                user.Username,
                user.Password,
                user.FirstName,
                user.LastName,
                user.Age,
                user.UserEmail,
                user.UserPhoneNr,
                user.StreetName,
                Role = user.Role.ToString()
            }).ConfigureAwait(false);

            return user;
        }
        public string CreateToken(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            Console.WriteLine($"Creating token for user: {user.Username}, Id: {user.UserId}, Role: {user.Role}");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
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

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user).ConfigureAwait(false)
            };
        }


        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.Id, request.RefreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid id, string refreshToken)
        {


            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE user_id = @UserId AND refresh_token = @RefreshToken AND refresh_token_expiry_time > @CurrentTime",
                new { UserId = id, RefreshToken = refreshToken, CurrentTime = DateTime.UtcNow });

            return user;
        }

        private static string GenerateRefreshToken()
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

            var sql = "UPDATE Users SET refresh_token = @RefreshToken, refresh_token_expiry_time = @RefreshTokenExpiryTime WHERE user_id = @UserId";
            await _dbConnection.ExecuteAsync(sql, new { user.RefreshToken, user.RefreshTokenExpiryTime, user.UserId });

            return refreshToken;
        }

    }
}
