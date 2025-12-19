using Microsoft.AspNetCore.Mvc;
using DataProcessing.Processing.Application.UseCases;
using DataProcessing.Processing.Api.Models;
using DataProcessing.Processing.Domain.Entities;
using DataProcessing.Monitoring;
namespace DataProcessing.Processing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    private readonly IngestMeasurementUseCase _useCase;

    public MeasurementsController(IngestMeasurementUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    public async Task<IActionResult> Ingest([FromBody] MeasurementRequest request)
    {

        using var activity = MonitorService.ActivitySource.StartActivity("Measurements.Ingest");

        if (string.IsNullOrWhiteSpace(request.Source))
            return BadRequest("Source cannot be empty");

        var measurement = new Measurement
        {
            Source = request.Source,
            Value = request.Value,
            Timestamp = DateTime.UtcNow
        };

        activity?.SetTag("measurement.source", measurement.Source);
        activity?.SetTag("measurement.value", measurement.Value);

        MonitorService.Log.Information(
        "[Processing] Received measurement: Source={Source}, Value={Value}, Timestamp={Timestamp}",
        measurement.Source, measurement.Value, measurement.Timestamp);

        await _useCase.ExecuteAsync(measurement);

        return Accepted();
    }
}
