using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessing.Ingestion.Application.DTOs;
namespace DataProcessing.Ingestion.Application.Interfaces;
public interface IProcessingClient
{
    void SendMeasurementAsync(MeasurementDto measurement);
}
