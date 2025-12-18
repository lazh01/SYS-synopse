namespace DataProcessing.Ingestion.Api.Models;

public class MeasurementRequest
{
    public string Source { get; set; } = string.Empty;
    public double Value { get; set; }
}
