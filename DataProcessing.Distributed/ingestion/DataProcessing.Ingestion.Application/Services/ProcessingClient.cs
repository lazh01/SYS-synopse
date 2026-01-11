using DataProcessing.Ingestion.Application.DTOs;
using DataProcessing.Ingestion.Application.Interfaces;
using System.Net.Http.Json;
using DataProcessing.Monitoring;
namespace DataProcessing.Ingestion.Application.Services
{
    public class ProcessingClient : IProcessingClient
    {
        private readonly HttpClient _http;

        public ProcessingClient(HttpClient http)
        {
            _http = http;
        }

        public void SendMeasurementAsync(MeasurementDto measurement)
        {

            using var activity = MonitorService.ActivitySource.StartActivity("ProcessingClient.SendMeasurement");

            activity?.SetTag("measurement.source", measurement.Source);
            activity?.SetTag("measurement.value", measurement.Value);

            try
            {
                var response = _http
                    .PostAsJsonAsync("/api/measurements", measurement)
                    .Result; // ⛔ BLOKERER tråden
                response.EnsureSuccessStatusCode();

                MonitorService.Log.Information(
                    "[ProcessingClient] Successfully sent measurement: {Source}={Value}",
                    measurement.Source, measurement.Value);
            }
            catch (Exception ex)
            {
                activity?.SetTag("error", true);
                MonitorService.Log.Error(ex, "[ProcessingClient] Failed to send measurement: {Source}={Value}",
                    measurement.Source, measurement.Value);
                throw;
            }
        }
    }
}