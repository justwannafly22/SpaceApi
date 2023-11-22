using IdentityApi.Infrastructure.Logic;
using IdentityApi.Repository;
using IdentityApi.Repository.Entities;
using IdentityApi.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityApi.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureLogic(this IServiceCollection services)
    {
        services.AddScoped<IIdentityLogic, IdentityLogic>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IIdentityRepository, IdentityRepository>();
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        string secret = Environment.GetEnvironmentVariable("ApiSecret")!;

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:ValidIssuer"],
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
            };
        });
    }
}
