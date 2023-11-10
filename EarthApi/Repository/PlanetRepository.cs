using Dapper;
using PlanetApi.Domain;
using PlanetApi.Infrastructure.Factories;
using PlanetApi.Repository.Entities;
using PlanetApi.Repository.Interfaces;
using System.Data;

namespace PlanetApi.Repository;

public class PlanetRepository : IPlanetRepository
{
    private readonly IDapperContext _context;
    private readonly IPlanetRepositoryFactory _planetFactory;

    private const string SP_CreatePlanet = "spCreatePlanet";
    private const string SP_UpdatePlanet = "spUpdatePlanet";
    private const string SP_GetPlanet = "spGetPlanet";
    private const string SP_GetPlanets = "spGetPlanets";
    private const string SP_DeletePlanet = "spDeletePlanet";
    private const string SP_GetPlanetByName = "spGetPlanetByName";

    public PlanetRepository(IDapperContext context, IPlanetRepositoryFactory planetFactory)
    {
        _context = context;
        _planetFactory = planetFactory;
    }

    public async Task<PlanetDomainModel> CreateAsync(PlanetDomainModel domainModel)
    {
        ArgumentNullException.ThrowIfNull(domainModel, nameof(domainModel));

        var planet = _planetFactory.ToEntity(domainModel);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(SP_CreatePlanet, new
        {
            planet.Name,
            planet.Location,
            planet.Age,
            planet.Air
        }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var insertedPlanet = await connection.QuerySingleOrDefaultAsync<Planet>(SP_GetPlanetByName, planet.Name, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return _planetFactory.ToDomain(insertedPlanet!);
    }

    public async Task<PlanetDomainModel> UpdateAsync(Guid id, PlanetDomainModel domainModel)
    {
        ArgumentNullException.ThrowIfNull(domainModel, nameof(domainModel));

        var planet = _planetFactory.ToEntity(domainModel);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(SP_UpdatePlanet, new
        {
            Id = id,
            planet.Name,
            planet.Location,
            planet.Age,
            planet.Air
        }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return _planetFactory.ToDomain(planet);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(SP_DeletePlanet, id, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    public async Task<List<PlanetDomainModel>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var planets = await connection.QueryAsync<Planet>(SP_GetPlanets, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var domainModels = planets.ToList().Select(planet => _planetFactory.ToDomain(planet));
        return domainModels.ToList();
    }

    public async Task<PlanetDomainModel> GetByIdAsync(Guid id)
    {
        using var connection = _context.CreateConnection();
        var planet = await connection.QuerySingleOrDefaultAsync<Planet>(SP_GetPlanet, id, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return _planetFactory.ToDomain(planet!);
    }
}
