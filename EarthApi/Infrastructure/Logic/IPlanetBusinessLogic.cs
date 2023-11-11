using PlanetApi.Domain;

namespace PlanetApi.Infrastructure.Logic;

public interface IPlanetBusinessLogic
{
    public Task<PlanetDomainModel> CreateAsync(PlanetDomainModel model);
    public Task<PlanetDomainModel> UpdateAsync(Guid id, PlanetDomainModel model);
    public Task DeleteAsync(Guid id);
    public Task<PlanetDomainModel> GetByIdAsync(Guid id);
    public Task<List<PlanetDomainModel>> GetAllPlanetsAsync();
}
