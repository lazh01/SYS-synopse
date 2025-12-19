using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessing.Processing.Application.Interfaces;
using DataProcessing.Processing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DataProcessing.Monitoring;
namespace DataProcessing.Processing.Infrastructure.Persistence;

public class MeasurementRepository : IMeasurementRepository
{
    private readonly ProcessingDbContext _context;

    public MeasurementRepository(ProcessingDbContext context)
    {
        _context = context;
    }

    public async Task AddMeasurementAsync(Measurement measurement)
    {
        using var activity = MonitorService.ActivitySource.StartActivity("Repository.AddMeasurement");
        activity?.SetTag("measurement.source", measurement.Source);

        _context.Measurements.Add(measurement);

        // Update or create aggregate
        var aggregate = await _context.SensorAggregates
            .FirstOrDefaultAsync(a => a.Source == measurement.Source);

        if (aggregate == null)
        {
            aggregate = new SensorAggregate { Source = measurement.Source };
            _context.SensorAggregates.Add(aggregate);
        }

        aggregate.Apply(measurement);

        await _context.SaveChangesAsync();

        MonitorService.Log.Information(
        "[Repository] Added measurement for {Source}, updated aggregates", measurement.Source);
    }

    public async Task<IReadOnlyList<SensorAggregate>> GetAggregatesAsync()
    {
        return await _context.SensorAggregates.AsNoTracking().ToListAsync();
    }
}
