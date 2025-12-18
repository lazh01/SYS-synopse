using DataProcessing.Dashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        var result = await _query.GetAllAsync();
        return Ok(result);
    }
}