using Dapper;
using PlanetApi.Domain;
using PlanetApi.Infrastructure.Exceptions;
using PlanetApi.Infrastructure.Factories;
using PlanetApi.Infrastructure.Logger;
using PlanetApi.Repository.Entities;
using PlanetApi.Repository.Interfaces;
using System.Data;

namespace PlanetApi.Repository;

public class PlanetRepository : IPlanetRepository
{
    private readonly ILoggerService _logger;
    private readonly IDapperContext _context;
    private readonly IPlanetRepositoryFactory _planetFactory;

    private const string SP_CreatePlanet = "spCreatePlanet";
    private const string SP_UpdatePlanet = "spUpdatePlanet";
    private const string SP_GetPlanet = "spGetPlanet";
    private const string SP_GetPlanets = "spGetPlanets";
    private const string SP_DeletePlanet = "spDeletePlanet";
    private const string SP_GetPlanetByName = "spGetPlanetByName";

    public PlanetRepository(IDapperContext context, IPlanetRepositoryFactory planetFactory, ILoggerService logger)
    {
        _logger = logger;
        _context = context;
        _planetFactory = planetFactory;
    }

    public async Task<PlanetDomainModel> CreateAsync(PlanetDomainModel domainModel)
    {
        ArgumentNullException.ThrowIfNull(domainModel, nameof(domainModel));

        var planet = _planetFactory.ToEntity(domainModel);

        using var connection = _context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(SP_CreatePlanet, new
        {
            planet.Name,
            planet.Location,
            planet.Age,
            planet.Air
        }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        if (affectedRows == 0)
        {
            _logger.LogError("Planet wasn`t created.");
            throw new Exception("Planet wasn`t created.");
        }

        var insertedPlanet = await connection.QuerySingleOrDefaultAsync<Planet>(SP_GetPlanetByName, new { planet.Name }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        _logger.LogInfo($"The planet: {planet.Name} created successfully.");

        return _planetFactory.ToDomain(insertedPlanet!);
    }

    public async Task<PlanetDomainModel> UpdateAsync(Guid id, PlanetDomainModel domainModel)
    {
        ArgumentNullException.ThrowIfNull(domainModel, nameof(domainModel));

        var planet = _planetFactory.ToEntity(domainModel);

        using var connection = _context.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(SP_UpdatePlanet, new
        {
            Id = id,
            planet.Name,
            planet.Location,
            planet.Age,
            planet.Air
        }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        if (rowsAffected == 0)
        {
            _logger.LogWarn($"The planet with id: {id} doesn`t exist in the database.");
            throw new NotFoundException($"The planet with id: {id} doesn`t exist in the database.");
        }

        return _planetFactory.ToDomain(planet);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _context.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(SP_DeletePlanet, new { id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        if (rowsAffected == 0)
        {
            _logger.LogWarn($"The planet with id: {id} doesn`t exist in the database.");
            throw new NotFoundException($"The planet with id: {id} doesn`t exist in the database.");
        }
    }

    public async Task<List<PlanetDomainModel>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var planets = await connection.QueryAsync<Planet>(SP_GetPlanets, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var domainModels = planets.ToList().Select(_planetFactory.ToDomain);
        return domainModels.ToList();
    }

    public async Task<PlanetDomainModel> GetByIdAsync(Guid id)
    {
        using var connection = _context.CreateConnection();

        var planet = await connection.QuerySingleOrDefaultAsync<Planet>(SP_GetPlanet, new { id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        if (planet is null)
        {
            _logger.LogWarn($"The planet with id: {id} doesn`t exist in the database.");
            throw new NotFoundException($"The planet with id: {id} doesn`t exist in the database.");
        }

        return _planetFactory.ToDomain(planet!);
    }
}
