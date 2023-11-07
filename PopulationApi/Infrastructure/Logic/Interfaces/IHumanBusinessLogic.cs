using MinimalApi.Domain;

namespace MinimalApi.Infrastructure.Logic.Interfaces;

public interface IHumanBusinessLogic
{
    public Task<HumanDomainModel> CreateAsync(HumanDomainModel model);
    public Task<HumanDomainModel> UpdateAsync(Guid id, HumanDomainModel model);
    public Task DeleteAsync(Guid id);
    public Task<HumanDomainModel> GetByIdAsync(Guid id);
    public Task<List<HumanDomainModel>> GetAllPeople();
}
