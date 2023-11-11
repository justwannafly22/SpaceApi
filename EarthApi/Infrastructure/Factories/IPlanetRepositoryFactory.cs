using PlanetApi.Domain;
using PlanetApi.Repository.Entities;

namespace PlanetApi.Infrastructure.Factories;

public interface IPlanetRepositoryFactory
{
    public PlanetDomainModel ToDomain(Planet planet);
    Planet ToEntity(PlanetDomainModel domainModel);
}
