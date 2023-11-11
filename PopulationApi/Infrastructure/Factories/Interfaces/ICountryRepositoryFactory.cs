using PopulationApi.Domain;
using PopulationApi.Repository.Entities;

namespace PopulationApi.Infrastructure.Factories.Interfaces;

public interface ICountryRepositoryFactory
{
    public CountryDomainModel ToDomain(Country country);
    public Country ToEntity(CountryDomainModel domain);
}
