using DataProcessing.Ingestion.Application.DTOs;
using DataProcessing.Ingestion.Application.Interfaces;
using DataProcessing.Monitoring;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DataProcessing.Ingestion.Application.Services;

public class MessageBrokerClient : IMessageBrokerClient, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "measurements";

    public MessageBrokerClient(IConnection connection)
    {
        _connection = connection;
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public Task PublishMeasurementAsync(MeasurementDto measurement)
    {
        using var activity = MonitorService.ActivitySource.StartActivity("MessageBrokerClient.PublishMeasurement");

        activity?.SetTag("measurement.source", measurement.Source);
        activity?.SetTag("measurement.value", measurement.Value);

        try
        {
            var message = JsonSerializer.Serialize(measurement);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);

            MonitorService.Log.Information(
                "[MessageBrokerClient] Published measurement to queue: {Source}={Value}",
                measurement.Source, measurement.Value);
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            MonitorService.Log.Error(ex, "[MessageBrokerClient] Failed to publish measurement: {Source}={Value}",
                measurement.Source, measurement.Value);
            throw;
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        // Note: We don't dispose the connection as it's a shared singleton
    }
}

