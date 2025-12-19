using DataProcessing.Dashboard.Application.Interfaces;
using DataProcessing.Monitoring;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DataProcessing.Dashboard.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly ISensorAggregateQuery _query;

    public DashboardController(ISensorAggregateQuery query)
    {
        _query = query;
    }

    [HttpGet("aggregates")]
    public async Task<IActionResult> GetAggregates()
    {
        using var activity = MonitorService.ActivitySource.StartActivity("DashboardController.GetAggregates");

        try
        {
            var result = await _query.GetAllAsync();
            activity?.SetTag("dashboard.aggregate.count", result.Count);
            MonitorService.Log.Information("[Dashboard] Returned {Count} aggregates", result.Count);
            return Ok(result);
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex, "[Dashboard] Error while fetching aggregates");
            throw;
        }
    }
}