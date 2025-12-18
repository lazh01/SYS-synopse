using DataProcessing.Processing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.Processing.Application.Interfaces;

public interface IMeasurementRepository
{
    Task AddMeasurementAsync(Measurement measurement);
    Task<IReadOnlyList<SensorAggregate>> GetAggregatesAsync();
}
