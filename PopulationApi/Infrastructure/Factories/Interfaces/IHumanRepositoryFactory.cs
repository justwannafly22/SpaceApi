using MinimalApi.Domain;
using MinimalApi.Repository.Entities;

namespace MinimalApi.Infrastructure.Factories.Interfaces;

public interface IHumanRepositoryFactory
{
    public HumanDomainModel ToDomain(Human human);
    public Human ToEntity(HumanDomainModel domain);
}
