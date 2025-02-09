using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SuperFarm.Infrastructure.Repositories.FarmRepositories;
using SuperFarm.Infrastructure.Repositories.UserRepositories;
using SuperFarm.Infrastructure.Repositories.ProductRepositories;
using Scalar.AspNetCore;
using SuperFarm.Services;
using System.Data;
using Npgsql;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());  // Converts enums to strings
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddOpenApi();

builder.Services.AddScoped<IFarmRepositories, FarmRepository>();
builder.Services.AddScoped<IUserRepositories, UserRepository>();
builder.Services.AddScoped<IProductRepositories, ProductRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<UserContextService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
        ValidateIssuerSigningKey = true
    };
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddAuthorization(options =>
{
    // Add policies for different roles
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer", "Admin", "Farmer"));
    options.AddPolicy("FarmerPolicy", policy => policy.RequireRole("Farmer", "Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();