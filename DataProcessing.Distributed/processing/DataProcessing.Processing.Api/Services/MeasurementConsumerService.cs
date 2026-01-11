using DataProcessing.Processing.Application.UseCases;
using DataProcessing.Processing.Domain.Entities;
using DataProcessing.Monitoring;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DataProcessing.Processing.Api.Services;

public class MeasurementConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private const string QueueName = "measurements";

    public MeasurementConsumerService(IConnection connection, IServiceProvider serviceProvider)
    {
        _connection = connection;
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            using var activity = MonitorService.ActivitySource.StartActivity("MeasurementConsumerService.ProcessMessage");

            try
            {
                var measurementDto = JsonSerializer.Deserialize<MeasurementDto>(message);
                
                if (measurementDto == null || string.IsNullOrWhiteSpace(measurementDto.Source))
                {
                    MonitorService.Log.Warning("[MeasurementConsumer] Received invalid measurement request");
                    _channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }

                activity?.SetTag("measurement.source", measurementDto.Source);
                activity?.SetTag("measurement.value", measurementDto.Value);

                MonitorService.Log.Information(
                    "[MeasurementConsumer] Received measurement from queue: Source={Source}, Value={Value}, Timestamp={Timestamp}",
                    measurementDto.Source, measurementDto.Value, measurementDto.Timestamp);

                // Create a scope to resolve scoped services
                using var scope = _serviceProvider.CreateScope();
                var useCase = scope.ServiceProvider.GetRequiredService<IngestMeasurementUseCase>();

                var measurement = new Measurement
                {
                    Source = measurementDto.Source,
                    Value = measurementDto.Value,
                    Timestamp = measurementDto.Timestamp
                };

                await useCase.ExecuteAsync(measurement);

                _channel.BasicAck(ea.DeliveryTag, false);

                MonitorService.Log.Information(
                    "[MeasurementConsumer] Successfully processed measurement: Source={Source}, Value={Value}",
                    measurementDto.Source, measurementDto.Value);
            }
            catch (Exception ex)
            {
                activity?.SetTag("error", true);
                MonitorService.Log.Error(ex, "[MeasurementConsumer] Error processing measurement: {Message}", message);
                // Reject and requeue the message
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

        MonitorService.Log.Information("[MeasurementConsumer] Started consuming messages from queue: {QueueName}", QueueName);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}

// DTO for deserializing the message from the queue
internal class MeasurementDto
{
    public string Source { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}

