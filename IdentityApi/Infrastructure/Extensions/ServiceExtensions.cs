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

    public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
}
