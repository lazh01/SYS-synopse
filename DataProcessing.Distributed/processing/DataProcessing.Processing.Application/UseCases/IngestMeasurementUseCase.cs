using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessing.Processing.Application.Interfaces;
using DataProcessing.Processing.Domain.Entities;
using DataProcessing.Monitoring;

namespace DataProcessing.Processing.Application.UseCases;

public class IngestMeasurementUseCase
{
    private readonly IMeasurementRepository _repository;

    public IngestMeasurementUseCase(IMeasurementRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Measurement measurement)
    {

        using var activity = MonitorService.ActivitySource.StartActivity("UseCase.IngestMeasurement");
        activity?.SetTag("measurement.source", measurement.Source);
        activity?.SetTag("measurement.value", measurement.Value);

        try
        {
            await _repository.AddMeasurementAsync(measurement);
            MonitorService.Log.Information(
                "[UseCase] Successfully ingested measurement: {Source}={Value}",
                measurement.Source, measurement.Value);
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex,
                "[UseCase] Failed to ingest measurement: {Source}={Value}",
                measurement.Source, measurement.Value);
            throw;
        }
    }
}