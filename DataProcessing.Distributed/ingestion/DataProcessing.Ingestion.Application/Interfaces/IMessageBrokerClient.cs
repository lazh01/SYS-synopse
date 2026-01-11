using DataProcessing.Ingestion.Application.DTOs;

namespace DataProcessing.Ingestion.Application.Interfaces;

public interface IMessageBrokerClient
{
    Task PublishMeasurementAsync(MeasurementDto measurement);
}

