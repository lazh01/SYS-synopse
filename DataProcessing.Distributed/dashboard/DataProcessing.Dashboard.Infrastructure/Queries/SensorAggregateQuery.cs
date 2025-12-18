using DataProcessing.Dashboard.Application.DTOs;
using DataProcessing.Dashboard.Application.Interfaces;
using DataProcessing.Dashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
        return await _db.SensorAggregates
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
    }
}