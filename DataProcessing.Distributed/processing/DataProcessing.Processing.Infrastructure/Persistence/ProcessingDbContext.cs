using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessing.Processing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProcessing.Processing.Infrastructure.Persistence;

public class ProcessingDbContext : DbContext
{
    public ProcessingDbContext(DbContextOptions<ProcessingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Measurement> Measurements { get; set; } = null!;
    public DbSet<SensorAggregate> SensorAggregates { get; set; } = null!;
}
