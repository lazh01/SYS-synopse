using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SensorData.Domain.Entities;

namespace SensorData.Application.Interfaces;

public interface ISensorAggregateRepository
{
    Task<SensorAggregate?> GetAsync(string source);
    Task SaveAsync(SensorAggregate aggregate);
    Task<IReadOnlyList<SensorAggregate>> GetAllAsync();
}
