using PlanetApi.Domain;

namespace PlanetApi.Repository.Interfaces;

public interface IPlanetRepository
{
    public Task<PlanetDomainModel> CreateAsync(PlanetDomainModel domainModel);
    public Task<PlanetDomainModel> UpdateAsync(Guid id, PlanetDomainModel domainModel);
    public Task DeleteAsync(Guid id);
    public Task<List<PlanetDomainModel>> GetAllAsync();
    public Task<PlanetDomainModel> GetByIdAsync(Guid id);
}
