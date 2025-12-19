using DataProcessing.Dashboard.Application.DTOs;
using DataProcessing.Dashboard.Application.Interfaces;
using DataProcessing.Dashboard.Infrastructure.Persistence;
using DataProcessing.Monitoring;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DataProcessing.Dashboard.Infrastructure.Queries;

public class SensorAggregateQuery : ISensorAggregateQuery
{
    private readonly DashboardDbContext _db;

    public SensorAggregateQuery(DashboardDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SensorAggregateDto>> GetAllAsync()
    {
        using var activity = MonitorService.ActivitySource.StartActivity("SensorAggregateQuery.GetAll");

        try
        {
            var result = await _db.SensorAggregates
                .AsNoTracking()
                .Select(a => new SensorAggregateDto
                {
                    Source = a.Source,
                    Count = a.Count,
                    Min = a.Min,
                    Max = a.Max,
                    Average = a.Average
                })
                .ToListAsync();

            activity?.SetTag("dashboard.aggregate.count", result.Count);
            MonitorService.Log.Information("[Dashboard] Fetched {Count} aggregates", result.Count);

            return result;
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex, "[Dashboard] Failed to fetch aggregates");
            throw;
        }
    }
}