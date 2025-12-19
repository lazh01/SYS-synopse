using Microsoft.AspNetCore.Mvc;
using SensorData.Application.Interfaces;
using SensorData.Domain.Entities;
using SensorData.Api.Models;
using DataProcessing.Monitoring;
using System.Diagnostics;

namespace SensorData.Api.Controllers;

[ApiController]
[Route("api/measurements")]
public class MeasurementsController : ControllerBase
{
    private readonly IIngestMeasurementUseCase _useCase;

    public MeasurementsController(IIngestMeasurementUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    public async Task<IActionResult> Ingest(IngestMeasurementRequest request)
    {
        using var activity = MonitorService.ActivitySource.StartActivity("MeasurementsController.Ingest");

        try
        {
            var measurement = new Measurement(
                request.Source,
                request.Value,
                DateTime.UtcNow
            );

            MonitorService.Log.Information(
                "[Measurements] Ingesting measurement: {Source}={Value}",
                measurement.Source, measurement.Value);

            await _useCase.ExecuteAsync(measurement);

            return Accepted();
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex, "[Measurements] Failed to ingest measurement: {Source}={Value}",
                request.Source, request.Value);
            throw;
        }
    }
}