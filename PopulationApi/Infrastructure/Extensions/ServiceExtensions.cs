using MinimalApi.Infrastructure.Factories;
using MinimalApi.Infrastructure.Factories.Interfaces;
using MinimalApi.Infrastructure.Logic;
using MinimalApi.Infrastructure.Logic.Interfaces;
using MinimalApi.Repository;
using MinimalApi.Repository.Interfaces;

namespace MinimalApi.Infrastructure.Extentions;

public static class ServiceExtensions
{
    public static void ConfigureFactories(this IServiceCollection services)
    {
        services.AddScoped<ICountryRepositoryFactory, CountryRepositoryFactory>();
        services.AddScoped<IHumanRepositoryFactory, HumanRepositoryFactory>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IHumanRepository, HumanRepository>();
    }

    public static void ConfigureLogic(this IServiceCollection services) 
    {
        services.AddScoped<IHumanBusinessLogic, HumanBusinessLogic>();
        services.AddScoped<ICountryBusinessLogic, CountryBusinessLogic>();
    }
}
