using Microsoft.EntityFrameworkCore;
using PopulationApi.Domain;
using PopulationApi.Infrastructure.Exceptions;
using PopulationApi.Infrastructure.Factories.Interfaces;
using PopulationApi.Repository.Entities;
using PopulationApi.Repository.Interfaces;
using Serilog;
using System.Linq.Expressions;

namespace PopulationApi.Repository;

public class HumanRepository : IHumanRepository
{
    private readonly AppDbContext _context;
    private readonly IHumanRepositoryFactory _factory;

    public HumanRepository(AppDbContext context, IHumanRepositoryFactory factory)
    {
        _context = context;
        _factory = factory;
    }

    public async Task<HumanDomainModel> CreateAsync(HumanDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var entity = _factory.ToEntity(model);
        await _context.People.AddAsync(entity);
        await _context.SaveChangesAsync();

        Log.Information($"The human: {entity} was successfully created.");

        return _factory.ToDomain(entity);
    }

    public async Task<List<HumanDomainModel>> CreateRangeAsync(List<HumanDomainModel> models)
    {
        ArgumentNullException.ThrowIfNull(models, nameof(models));

        var entities = models.Select(_factory.ToEntity);
        await _context.People.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        Log.Information($"Amount of people: {entities.Count()} were successfully created.");

        return entities.Select(_factory.ToDomain).ToList();
    }

    public async Task DeleteAsync(Guid? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));

        var entity = await GetHumanByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync();
        if (entity is null)
        {
            Log.Warning($"The human with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The human with id {id} doesn`t exist in the database.");
        }

        _context.Remove(entity!);

        await _context.SaveChangesAsync();

        Log.Information($"The human: {entity} was successfully deleted.");
    }

    public async Task<List<HumanDomainModel>> GetAllAsync()
    {
        var entities = await GetAllCountries().Select(e => _factory.ToDomain(e)).AsNoTracking().ToListAsync();

        Log.Information($"The human table was triggered.");

        return entities;
    }

    public async Task<HumanDomainModel> GetByIdAsync(Guid? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));

        var entity = await GetHumanByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync();
        if (entity is null)
        {
            Log.Warning($"The human with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The human with id {id} doesn`t exist in the database.");
        }

        return _factory.ToDomain(entity!);
    }

    public async Task<HumanDomainModel> UpdateAsync(Guid id, HumanDomainModel model)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var entity = await GetHumanByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync();
        if (entity is null)
        {
            Log.Warning($"The human with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The human with id {id} doesn`t exist in the database.");
        }

        entity.Name = model.Name;
        entity.Surname = model.Surname;
        entity.Age = model.Age;
        entity.Gender = model.Gender;

        await _context.SaveChangesAsync();

        Log.Information($"The human: {entity} was successfully updated.");

        return _factory.ToDomain(entity);
    }

    private IQueryable<Human> GetAllCountries() =>
        _context.Set<Human>();

    private IQueryable<Human> GetHumanByExpression(Expression<Func<Human, bool>> expression) =>
        _context.Set<Human>()
                .Where(expression);
}
