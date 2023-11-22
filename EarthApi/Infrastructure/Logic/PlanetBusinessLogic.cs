using PlanetApi.Domain;
using PlanetApi.Repository.Interfaces;

namespace PlanetApi.Infrastructure.Logic;

public class PlanetBusinessLogic : IPlanetBusinessLogic
{
    private readonly IPlanetRepository _repository;

    public PlanetBusinessLogic(IPlanetRepository repository)
    {
        _repository = repository;
    }

    public async Task<PlanetDomainModel> CreateAsync(PlanetDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return await _repository.CreateAsync(model);
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
