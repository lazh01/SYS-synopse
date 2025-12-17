using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorData.Domain.Entities;

public class SensorAggregate
{
    public string Source { get; private set; } = null!;
    public int Count { get; private set; }
    public double Min { get; private set; }
    public double Max { get; private set; }
    public double Sum { get; private set; }

    public double Average => Count == 0 ? 0 : Sum / Count;

    private SensorAggregate() { } // for EF

    public SensorAggregate(string source)
    {
        Source = source;
        Count = 0;
        Min = double.MaxValue;
        Max = double.MinValue;
        Sum = 0;
    }

    public void Apply(double value)
    {
        Count++;
        Sum += value;
        Min = Math.Min(Min, value);
        Max = Math.Max(Max, value);
    }
}
