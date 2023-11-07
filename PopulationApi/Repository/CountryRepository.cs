﻿using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain;
using MinimalApi.Infrastructure.Exceptions;
using MinimalApi.Infrastructure.Factories.Interfaces;
using MinimalApi.Repository.Entities;
using MinimalApi.Repository.Interfaces;
using Serilog;
using System.Linq.Expressions;

namespace MinimalApi.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _context;
    private readonly ICountryRepositoryFactory _factory;

    public CountryRepository(AppDbContext context, ICountryRepositoryFactory factory)
    {
        _context = context;
        _factory = factory;
    }

    public async Task<CountryDomainModel> CreateAsync(CountryDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var entity = _factory.ToEntity(model);
        await _context.Countries.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        Log.Information($"The country: {entity} was successfully created.");

        return _factory.ToDomain(entity);
    }

    public async Task DeleteAsync(Guid? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));

        var entity = await GetCountryByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync().ConfigureAwait(false);
        if (entity is null)
        {
            Log.Warning($"The country with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The country with id {id} doesn`t exist in the database.");
        }

        _context.Remove(entity!);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        Log.Information($"The country: {entity} was successfully deleted.");
    }

    public async Task<List<CountryDomainModel>> GetAllAsync()
    {
        var entities = await GetAllCountries().Select(e => _factory.ToDomain(e)).AsNoTracking().ToListAsync().ConfigureAwait(false);

        Log.Information($"The country table was triggered.");

        return entities;
    }

    public async Task<CountryDomainModel> GetByIdAsync(Guid? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id.ToString(), nameof(id));

        var entity = await GetCountryByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync().ConfigureAwait(false);
        if (entity is null)
        {
            Log.Warning($"The country with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The country with id {id} doesn`t exist in the database.");
        }

        return _factory.ToDomain(entity!);
    }

    public async Task<CountryDomainModel> UpdateAsync(Guid id, CountryDomainModel model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        var entity = await GetCountryByExpression(e => e.Id.Equals(id)).SingleOrDefaultAsync().ConfigureAwait(false);
        if (entity is null)
        {
            Log.Warning($"The country with id {id} doesn`t exist in the database.");
            throw new NotFoundException($"The country with id {id} doesn`t exist in the database.");
        }

        entity.Name = model.Name;
        entity.Square = model.Square;
        entity.Population = model.Population;

        await _context.SaveChangesAsync().ConfigureAwait(false);

        Log.Information($"The country: {entity} was successfully updated.");

        return _factory.ToDomain(entity);
    }

    private IQueryable<Country> GetAllCountries() =>
        _context.Set<Country>();

    private IQueryable<Country> GetCountryByExpression(Expression<Func<Country, bool>> expression) =>
        _context.Set<Country>()
                .Where(expression);
}
