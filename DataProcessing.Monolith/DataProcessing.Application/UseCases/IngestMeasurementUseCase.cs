using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SensorData.Application.Interfaces;
using SensorData.Domain.Entities;

namespace SensorData.Application.UseCases;

public class IngestMeasurementUseCase : IIngestMeasurementUseCase
{
    private readonly ISensorAggregateRepository _repository;

    public IngestMeasurementUseCase(ISensorAggregateRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Measurement measurement)
    {
        var aggregate = await _repository.GetAsync(measurement.Source)
                       ?? new SensorAggregate(measurement.Source);

        aggregate.Apply(measurement.Value);

        await _repository.SaveAsync(aggregate);
    }
}
