using DataProcessing.Ingestion.Application.DTOs;

namespace DataProcessing.Ingestion.Application.Interfaces;

public interface IAsyncProcessingClient
{
    Task SendMeasurementAsync(MeasurementDto measurement);
}

