using MinimalApi.Domain;
using MinimalApi.Infrastructure.Factories.Interfaces;
using MinimalApi.Repository.Entities;

namespace MinimalApi.Infrastructure.Factories;

public class CountryRepositoryFactory : ICountryRepositoryFactory
{
    public CountryDomainModel ToDomain(Country country)
    {
        ArgumentNullException.ThrowIfNull(country, nameof(country));

        return new CountryDomainModel
        {
            Id = country.Id,
            Name = country.Name,
            Population = country.Population,
            Square = country.Square
        };
    }

    public Country ToEntity(CountryDomainModel domain)
    {
        ArgumentNullException.ThrowIfNull(domain, nameof(domain));

        return new Country
        {
            Id = domain.Id,
            Name = domain.Name,
            Population = domain.Population,
            Square = domain.Square
        };
    }
}
