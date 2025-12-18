using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.Dashboard.Domain;

public class Measurement
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public string Source { get; set; } = null!;
}
