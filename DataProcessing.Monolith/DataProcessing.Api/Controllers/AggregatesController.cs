using Microsoft.AspNetCore.Mvc;
using SensorData.Application.Interfaces;

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
        var aggregates = await _repository.GetAllAsync();
        return Ok(aggregates);
    }
}