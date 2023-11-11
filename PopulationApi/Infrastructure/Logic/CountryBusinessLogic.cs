using PopulationApi.Domain;
using PopulationApi.Infrastructure.Logic.Interfaces;
using PopulationApi.Repository.Interfaces;

namespace PopulationApi.Infrastructure.Logic;

public class CountryBusinessLogic : ICountryBusinessLogic
{
    private readonly ICountryRepository _repository;

    public CountryBusinessLogic(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CountryDomainModel> CreateAsync(CountryDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return await _repository.CreateAsync(model).ConfigureAwait(false);
    }
    
    public async Task<CountryDomainModel> UpdateAsync(Guid id, CountryDomainModel model)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        return await _repository.UpdateAsync(id, model).ConfigureAwait(false);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id).ConfigureAwait(false);
    }

    public async Task<CountryDomainModel> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id).ConfigureAwait(false);
    }

    public async Task<List<CountryDomainModel>> GetAllCountriesAsync()
    {
        return await _repository.GetAllAsync().ConfigureAwait(false);
    }
}
