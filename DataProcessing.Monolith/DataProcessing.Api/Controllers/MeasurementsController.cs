using Microsoft.AspNetCore.Mvc;
using SensorData.Api.Models;
using SensorData.Application.Interfaces;
using SensorData.Domain.Entities;

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
        var measurement = new Measurement(
            request.Source,
            request.Value,
            DateTime.UtcNow
        );

        await _useCase.ExecuteAsync(measurement);

        return Accepted();
    }
}