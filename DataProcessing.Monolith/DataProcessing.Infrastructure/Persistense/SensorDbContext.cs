using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SensorData.Domain.Entities;

namespace SensorData.Infrastructure.Persistence;

public class SensorDbContext : DbContext
{
    public SensorDbContext(DbContextOptions<SensorDbContext> options)
        : base(options) { }

    public DbSet<SensorAggregate> SensorAggregates => Set<SensorAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SensorAggregate>()
            .HasKey(x => x.Source);

        base.OnModelCreating(modelBuilder);
    }
}
