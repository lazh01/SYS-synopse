namespace DataProcessing.Dashboard.Domain.ReadModels;

public class SensorAggregateReadModel
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public double Average { get; set; }
}