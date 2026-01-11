using DataProcessing.Ingestion.Application.DTOs;
using DataProcessing.Ingestion.Application.Interfaces;
using System.Net.Http.Json;
using DataProcessing.Monitoring;

namespace DataProcessing.Ingestion.Application.Services;

public class AsyncProcessingClient : IAsyncProcessingClient
{
    private readonly HttpClient _http;

    public AsyncProcessingClient(HttpClient http)
    {
        _http = http;
    }

    public Task SendMeasurementAsync(MeasurementDto measurement)
    {
        using var activity = MonitorService.ActivitySource.StartActivity("AsyncProcessingClient.SendMeasurement");

        activity?.SetTag("measurement.source", measurement.Source);
        activity?.SetTag("measurement.value", measurement.Value);

        // Fire and forget - don't await the response
        _ = Task.Run(async () =>
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/measurements", measurement);
                response.EnsureSuccessStatusCode();

                MonitorService.Log.Information(
                    "[AsyncProcessingClient] Successfully sent measurement: {Source}={Value}",
                    measurement.Source, measurement.Value);
            }
            catch (Exception ex)
            {
                activity?.SetTag("error", true);
                MonitorService.Log.Error(ex, "[AsyncProcessingClient] Failed to send measurement: {Source}={Value}",
                    measurement.Source, measurement.Value);
            }
        });

        MonitorService.Log.Information(
            "[AsyncProcessingClient] Queued measurement for async processing: {Source}={Value}",
            measurement.Source, measurement.Value);

        return Task.CompletedTask;
    }
}

