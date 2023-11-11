using PlanetApi.Infrastructure.Factories;
using PlanetApi.Infrastructure.Logic;
using PlanetApi.Repository;
using PlanetApi.Repository.Interfaces;

namespace PlanetApi.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureFactories(this IServiceCollection services)
    {
        services.AddScoped<IPlanetRepositoryFactory, PlanetRepositoryFactory>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IDapperContext, DapperContext>();
        services.AddScoped<IPlanetRepository, PlanetRepository>();
    }

    public static void ConfigureLogic(this IServiceCollection services)
    {
        services.AddScoped<IPlanetBusinessLogic, PlanetBusinessLogic>();
    }
}
