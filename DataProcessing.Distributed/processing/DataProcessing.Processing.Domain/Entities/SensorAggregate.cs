using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.Processing.Domain.Entities;

public class SensorAggregate
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Min { get; set; } = double.MaxValue;
    public double Max { get; set; } = double.MinValue;
    public double Sum { get; set; }

    public double Average => Count == 0 ? 0 : Sum / Count;

    public void Apply(Measurement m)
    {
        Count++;
        Sum += m.Value;
        Min = Math.Min(Min, m.Value);
        Max = Math.Max(Max, m.Value);
    }
}
