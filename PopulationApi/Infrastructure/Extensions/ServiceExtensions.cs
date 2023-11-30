using MassTransit;
using PopulationApi.Infrastructure.Factories;
using PopulationApi.Infrastructure.Factories.Interfaces;
using PopulationApi.Infrastructure.Logic;
using PopulationApi.Infrastructure.Logic.Interfaces;
using PopulationApi.Repository;
using PopulationApi.Repository.Interfaces;

namespace PopulationApi.Infrastructure.Extentions;

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

    public static void ConfigureMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(_ =>
        {
            _.SetKebabCaseEndpointNameFormatter();

            var assembly = typeof(Program).Assembly;
            _.AddConsumers(assembly);
            _.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
