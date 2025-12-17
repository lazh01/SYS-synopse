using Microsoft.EntityFrameworkCore;
using SensorData.Application.Interfaces;
using SensorData.Domain.Entities;

namespace SensorData.Infrastructure.Persistence;

public class SensorAggregateRepository : ISensorAggregateRepository
{
    private readonly SensorDbContext _context;

    public SensorAggregateRepository(SensorDbContext context)
    {
        _context = context;
    }

    public async Task<SensorAggregate?> GetAsync(string source)
    {
        return await _context.SensorAggregates
            .FirstOrDefaultAsync(x => x.Source == source);
    }

    public async Task SaveAsync(SensorAggregate aggregate)
    {
        var exists = await _context.SensorAggregates
            .AnyAsync(x => x.Source == aggregate.Source);

        if (!exists)
        {
            _context.SensorAggregates.Add(aggregate);
        }
        // else: entity already tracked or loaded → no Update call needed

        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<SensorAggregate>> GetAllAsync()
    {
        return await _context.SensorAggregates.ToListAsync();
    }
}