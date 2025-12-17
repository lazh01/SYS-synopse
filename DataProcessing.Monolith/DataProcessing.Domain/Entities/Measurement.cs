using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorData.Domain.Entities;

public class Measurement
{
    public string Source { get; }
    public double Value { get; }
    public DateTime Timestamp { get; }

    public Measurement(string source, double value, DateTime timestamp)
    {
        Source = source;
        Value = value;
        Timestamp = timestamp;
    }
}
