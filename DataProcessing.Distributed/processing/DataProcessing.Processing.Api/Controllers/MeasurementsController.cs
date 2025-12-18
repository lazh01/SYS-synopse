using Microsoft.AspNetCore.Mvc;
using DataProcessing.Processing.Application.UseCases;
using DataProcessing.Processing.Api.Models;
using DataProcessing.Processing.Domain.Entities;

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
        var measurement = new Measurement
        {
            Source = request.Source,
            Value = request.Value,
            Timestamp = DateTime.UtcNow
        };

        await _useCase.ExecuteAsync(measurement);

        return Accepted();
    }
}
