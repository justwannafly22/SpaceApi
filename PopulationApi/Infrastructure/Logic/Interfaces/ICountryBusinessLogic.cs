using PopulationApi.Domain;

namespace PopulationApi.Infrastructure.Logic.Interfaces;

public interface ICountryBusinessLogic
{
    public Task<CountryDomainModel> CreateAsync(CountryDomainModel model);
    public Task<CountryDomainModel> UpdateAsync(Guid id, CountryDomainModel model);
    public Task DeleteAsync(Guid id);
    public Task<CountryDomainModel> GetByIdAsync(Guid id);
    public Task<List<CountryDomainModel>> GetAllCountriesAsync();
}
