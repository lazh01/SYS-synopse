using DataProcessing.Ingestion.Api.Models;
using DataProcessing.Ingestion.Application.DTOs;
using DataProcessing.Ingestion.Application.Interfaces;
using DataProcessing.Monitoring;
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

        using var activity = MonitorService.ActivitySource.StartActivity("Measurements.Ingest");

        if (string.IsNullOrWhiteSpace(request.Source))
            return BadRequest("Source cannot be empty");
        
        

        var timestamp = DateTime.UtcNow;

        activity?.SetTag("measurement.source", request.Source);
        activity?.SetTag("measurement.value", request.Value);
        MonitorService.Log.Information(
            "[Ingestion] Received measurement: Source={Source}, Value={Value}, Timestamp={Timestamp}",
            request.Source, request.Value, timestamp);


        await _processing.SendMeasurementAsync(new MeasurementDto
        {
            Source = request.Source,
            Value = request.Value,
            Timestamp = timestamp
        });

        return Accepted();
    }
}