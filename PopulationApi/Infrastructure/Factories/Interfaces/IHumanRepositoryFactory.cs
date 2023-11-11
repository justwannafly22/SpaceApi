using PopulationApi.Domain;
using PopulationApi.Repository.Entities;

namespace PopulationApi.Infrastructure.Factories.Interfaces;

public interface IHumanRepositoryFactory
{
    public HumanDomainModel ToDomain(Human human);
    public Human ToEntity(HumanDomainModel domain);
}
