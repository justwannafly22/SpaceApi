using MassTransit;
using Models;
using NLog.Fluent;
using PlanetApi.Domain;
using PlanetApi.Infrastructure.Logger;
using PlanetApi.Repository.Interfaces;

namespace PlanetApi.Infrastructure.Logic;

public class PlanetBusinessLogic : IPlanetBusinessLogic
{
    private readonly IPlanetRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILoggerService _logger;

    public PlanetBusinessLogic(IPlanetRepository repository, IPublishEndpoint publishEndpoint, ILoggerService logger)
    {
        _logger = logger;
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<PlanetDomainModel> CreateAsync(PlanetDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var planet = await _repository.CreateAsync(model);

        await _publishEndpoint.Publish<PlanetCreated>(new
        {
            planet.Id, DateTime.Now
        });

        _logger.LogInfo($"Planet with id: {planet.Id} just created.");

        return planet;
    }

    public async Task<PlanetDomainModel> UpdateAsync(Guid id, PlanetDomainModel model)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return await _repository.UpdateAsync(id, model);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PlanetDomainModel> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<PlanetDomainModel>> GetAllPlanetsAsync()
    {
        return await _repository.GetAllAsync();
    }
}
