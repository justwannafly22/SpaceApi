using MinimalApi.Domain;

namespace MinimalApi.Repository.Interfaces;

public interface IHumanRepository
{
    public Task<HumanDomainModel> CreateAsync(HumanDomainModel model);
    public Task DeleteAsync(Guid? id);
    Task<List<HumanDomainModel>> GetAllAsync();
    Task<HumanDomainModel> GetByIdAsync(Guid? id);
    Task<HumanDomainModel> UpdateAsync(Guid id, HumanDomainModel model);
}
