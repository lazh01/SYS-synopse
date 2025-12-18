namespace DataProcessing.Ingestion.Domain.Entities;

public class Measurement
{
    public int Id { get; set; } // EF Core primary key
    public string Source { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}
