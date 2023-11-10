using PlanetApi.Domain;
using PlanetApi.Repository.Entities;

namespace PlanetApi.Infrastructure.Factories;

public class PlanetRepositoryFactory : IPlanetRepositoryFactory
{
    public PlanetDomainModel ToDomain(Planet planet)
    {
        ArgumentNullException.ThrowIfNull(planet, nameof(planet));

        return new PlanetDomainModel
        {
            Id = planet.Id,
            Name = planet.Name,
            Age = planet.Age,
            Location = planet.Location,
            Air = planet.Air
        };
    }

    public Planet ToEntity(PlanetDomainModel domainModel)
    {
        ArgumentNullException.ThrowIfNull(domainModel, nameof(domainModel));

        return new Planet
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            Age = domainModel.Age,
            Location = domainModel.Location,
            Air = domainModel.Air
        };
    }
}
