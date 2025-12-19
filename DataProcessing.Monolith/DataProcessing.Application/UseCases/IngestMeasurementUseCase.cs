using SensorData.Application.Interfaces;
using SensorData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessing.Monitoring;
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
        using var activity = MonitorService.ActivitySource.StartActivity("IngestMeasurementUseCase.Execute");

        try
        {
            var aggregate = await _repository.GetAsync(measurement.Source)
                           ?? new SensorAggregate(measurement.Source);

            aggregate.Apply(measurement.Value);

            await _repository.SaveAsync(aggregate);

            MonitorService.Log.Information(
                "[UseCase] Applied measurement: {Source}={Value}",
                measurement.Source, measurement.Value);
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex, "[UseCase] Error applying measurement: {Source}={Value}",
                measurement.Source, measurement.Value);
            throw;
        }
    }
}
