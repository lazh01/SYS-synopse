using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SensorData.Domain.Entities;

namespace SensorData.Application.Interfaces;

public interface IIngestMeasurementUseCase
{
    Task ExecuteAsync(Measurement measurement);
}
