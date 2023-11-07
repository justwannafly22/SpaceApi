﻿using Microsoft.EntityFrameworkCore;
using MinimalApi.Repository.Entities;

namespace MinimalApi.Repository;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Human> People { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Human>()
            .HasOne(x => x.Country)
            .WithMany(x => x.People);
    }
}
