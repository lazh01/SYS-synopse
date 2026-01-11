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
    private readonly IProcessingClient _syncProcessing;
    private readonly IAsyncProcessingClient _asyncProcessing;
    private readonly IMessageBrokerClient _messageBroker;

    public MeasurementsController(
        IProcessingClient syncProcessing,
        IAsyncProcessingClient asyncProcessing,
        IMessageBrokerClient messageBroker)
    {
        _syncProcessing = syncProcessing;
        _asyncProcessing = asyncProcessing;
        _messageBroker = messageBroker;
    }

    [HttpPost("sync")]
    public async Task<IActionResult> IngestSync([FromBody] MeasurementRequest request)
    {
        using var activity = MonitorService.ActivitySource.StartActivity("Measurements.IngestSync");

        if (string.IsNullOrWhiteSpace(request.Source))
            return BadRequest("Source cannot be empty");

        var timestamp = DateTime.UtcNow;

        activity?.SetTag("measurement.source", request.Source);
        activity?.SetTag("measurement.value", request.Value);
        MonitorService.Log.Information(
            "[Ingestion] Received measurement (sync): Source={Source}, Value={Value}, Timestamp={Timestamp}",
            request.Source, request.Value, timestamp);

        try
        {
            await _syncProcessing.SendMeasurementAsync(new MeasurementDto
            {
                Source = request.Source,
                Value = request.Value,
                Timestamp = timestamp
            });

            return Ok(new { message = "Measurement sent synchronously", source = request.Source, value = request.Value });
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex, "[Ingestion] Error sending measurement synchronously");
            return StatusCode(500, new { error = "Failed to send measurement" });
        }
    }

    [HttpPost("async")]
    public IActionResult IngestAsync([FromBody] MeasurementRequest request)
    {
        using var activity = MonitorService.ActivitySource.StartActivity("Measurements.IngestAsync");

        if (string.IsNullOrWhiteSpace(request.Source))
            return BadRequest("Source cannot be empty");

        var timestamp = DateTime.UtcNow;

        activity?.SetTag("measurement.source", request.Source);
        activity?.SetTag("measurement.value", request.Value);
        MonitorService.Log.Information(
            "[Ingestion] Received measurement (async): Source={Source}, Value={Value}, Timestamp={Timestamp}",
            request.Source, request.Value, timestamp);

        _asyncProcessing.SendMeasurementAsync(new MeasurementDto
        {
            Source = request.Source,
            Value = request.Value,
            Timestamp = timestamp
        });

        return Accepted(new { message = "Measurement queued for async processing", source = request.Source, value = request.Value });
    }

    [HttpPost("broker")]
    public async Task<IActionResult> IngestBroker([FromBody] MeasurementRequest request)
    {
        using var activity = MonitorService.ActivitySource.StartActivity("Measurements.IngestBroker");

        if (string.IsNullOrWhiteSpace(request.Source))
            return BadRequest("Source cannot be empty");

        var timestamp = DateTime.UtcNow;

        activity?.SetTag("measurement.source", request.Source);
        activity?.SetTag("measurement.value", request.Value);
        MonitorService.Log.Information(
            "[Ingestion] Received measurement (broker): Source={Source}, Value={Value}, Timestamp={Timestamp}",
            request.Source, request.Value, timestamp);

        try
        {
            await _messageBroker.PublishMeasurementAsync(new MeasurementDto
            {
                Source = request.Source,
                Value = request.Value,
                Timestamp = timestamp
            });

            return Accepted(new { message = "Measurement published to message broker", source = request.Source, value = request.Value });
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex, "[Ingestion] Error publishing measurement to message broker");
            return StatusCode(500, new { error = "Failed to publish measurement" });
        }
    }
}