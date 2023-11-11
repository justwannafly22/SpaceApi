using PopulationApi.Domain;
using PopulationApi.Infrastructure.Logic.Interfaces;
using PopulationApi.Repository.Interfaces;

namespace PopulationApi.Infrastructure.Logic;

public class HumanBusinessLogic : IHumanBusinessLogic
{
    private readonly IHumanRepository _repository;

    public HumanBusinessLogic(IHumanRepository repository)
    {
        _repository = repository;
    }

    public async Task<HumanDomainModel> CreateAsync(HumanDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return await _repository.CreateAsync(model).ConfigureAwait(false);
    }

    public async Task<HumanDomainModel> UpdateAsync(Guid id, HumanDomainModel model)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return await _repository.UpdateAsync(id, model).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id).ConfigureAwait(false);
    }

    public async Task<HumanDomainModel> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id).ConfigureAwait(false);
    }

    public async Task<List<HumanDomainModel>> GetAllPeopleAsync()
    {
        return await _repository.GetAllAsync().ConfigureAwait(false);
    }
}
