using MinimalApi.Domain;
using MinimalApi.Repository.Entities;

namespace MinimalApi.Infrastructure.Factories.Interfaces;

public interface ICountryRepositoryFactory
{
    public CountryDomainModel ToDomain(Country country);
    public Country ToEntity(CountryDomainModel domain);
}
