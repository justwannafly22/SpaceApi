using PopulationApi.Domain;
using PopulationApi.Infrastructure.Factories.Interfaces;
using PopulationApi.Repository.Entities;

namespace PopulationApi.Infrastructure.Factories;

public class HumanRepositoryFactory : IHumanRepositoryFactory
{
    public HumanDomainModel ToDomain(Human human)
    {
        ArgumentNullException.ThrowIfNull(human, nameof(human));

        return new HumanDomainModel
        {
            Age = human.Age,
            Gender = human.Gender,
            Id = human.Id,
            Name = human.Name,
            Surname = human.Surname,
            CountryId = human.CountryId
        };
    }

    public Human ToEntity(HumanDomainModel domain)
    {
        ArgumentNullException.ThrowIfNull(domain, nameof(domain));

        return new Human
        {
            Age = domain.Age,
            Surname = domain.Surname,
            Name = domain.Name,
            Id = domain.Id,
            Gender = domain.Gender,
            CountryId = domain.CountryId
        };
    }
}
