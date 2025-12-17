namespace SensorData.Api.Models;

public class IngestMeasurementRequest
{
    public string Source { get; set; } = string.Empty;
    public double Value { get; set; }
}
