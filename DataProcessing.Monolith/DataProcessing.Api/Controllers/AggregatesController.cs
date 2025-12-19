using Microsoft.AspNetCore.Mvc;
using SensorData.Application.Interfaces;
using DataProcessing.Monitoring;
using System.Diagnostics;

namespace SensorData.Api.Controllers;

[ApiController]
[Route("api/aggregates")]
public class AggregatesController : ControllerBase
{
    private readonly ISensorAggregateRepository _repository;

    public AggregatesController(ISensorAggregateRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        using var activity = MonitorService.ActivitySource.StartActivity("AggregatesController.Get");

        try
        {
            var aggregates = await _repository.GetAllAsync();

            MonitorService.Log.Information(
                "[Aggregates] Retrieved {Count} aggregates at {Time}",
                aggregates.Count, DateTime.UtcNow);

            return Ok(aggregates);
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex, "[Aggregates] Failed to retrieve aggregates");
            throw;
        }
    }
}