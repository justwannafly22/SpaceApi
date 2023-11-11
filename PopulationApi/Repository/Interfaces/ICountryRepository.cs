using PopulationApi.Domain;

namespace PopulationApi.Repository.Interfaces;

public interface ICountryRepository
{
    public Task<CountryDomainModel> CreateAsync(CountryDomainModel model);
    public Task DeleteAsync(Guid? id);
    Task<List<CountryDomainModel>> GetAllAsync();
    Task<CountryDomainModel> GetByIdAsync(Guid? id);
    Task<CountryDomainModel> UpdateAsync(Guid id, CountryDomainModel model);
}
