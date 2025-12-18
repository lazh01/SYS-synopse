using DataProcessing.Ingestion.Api.Models;
using DataProcessing.Ingestion.Application.DTOs;
using DataProcessing.Ingestion.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessing.Ingestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    private readonly IProcessingClient _processing;

    public MeasurementsController(IProcessingClient processing)
    {
        _processing = processing;
    }

    [HttpPost]
    public async Task<IActionResult> Ingest([FromBody] MeasurementRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Source))
            return BadRequest("Source cannot be empty");

        // Set timestamp now
        var timestamp = DateTime.UtcNow;

        Console.WriteLine($"[Ingestion] Received measurement: Source={request.Source}, Value={request.Value}, Timestamp={timestamp}");

        // Forward to Processing
        await _processing.SendMeasurementAsync(new MeasurementDto
        {
            Source = request.Source,
            Value = request.Value,
            Timestamp = timestamp
        });

        return Accepted();
    }
}