using Microsoft.AspNetCore.Mvc;
using DataProcessing.Ingestion.Api.Models;

namespace DataProcessing.Ingestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    [HttpPost]
    public IActionResult Ingest([FromBody] MeasurementRequest request)
    {
        // Validate minimal
        if (string.IsNullOrWhiteSpace(request.Source))
            return BadRequest("Source cannot be empty");

        // Log for now
        Console.WriteLine($"[Ingestion] Received measurement: Source={request.Source}, Value={request.Value}, Timestamp={DateTime.UtcNow}");

        // Forwarding will be implemented later

        return Accepted();
    }
}