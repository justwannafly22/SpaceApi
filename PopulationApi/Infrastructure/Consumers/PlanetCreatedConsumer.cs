using MassTransit;
using Models;
using Serilog;

namespace PopulationApi.Infrastructure.Consumers;

public class PlanetCreatedConsumer : IConsumer<PlanetCreated>
{
    public Task Consume(ConsumeContext<PlanetCreated> context)
    {
        Log.Information($"Planet created with id: {context.Message.Id} at {context.Message.CreatedAt}");

        return Task.CompletedTask;
    }
}
