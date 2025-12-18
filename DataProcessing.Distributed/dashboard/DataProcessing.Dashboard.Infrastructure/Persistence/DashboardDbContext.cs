using DataProcessing.Dashboard.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace DataProcessing.Dashboard.Infrastructure.Persistence;

public class DashboardDbContext : DbContext
{
    public DashboardDbContext(DbContextOptions<DashboardDbContext> options)
        : base(options) { }

    public DbSet<SensorAggregateReadModel> SensorAggregates => Set<SensorAggregateReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DashboardDbContext).Assembly);
    }
}