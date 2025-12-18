using DataProcessing.Dashboard.Application.DTOs;

namespace DataProcessing.Dashboard.Application.Interfaces;

public interface ISensorAggregateQuery
{
    Task<IReadOnlyList<SensorAggregateDto>> GetAllAsync();
}
