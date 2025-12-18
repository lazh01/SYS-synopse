using DataProcessing.Ingestion.Application.DTOs;
using DataProcessing.Ingestion.Application.Interfaces;
using System.Net.Http.Json;

namespace DataProcessing.Ingestion.Application.Services
{
    public class ProcessingClient : IProcessingClient
    {
        private readonly HttpClient _http;

        public ProcessingClient(HttpClient http)
        {
            _http = http;
        }

        public async Task SendMeasurementAsync(MeasurementDto measurement)
        {
            var response = await _http.PostAsJsonAsync("/api/measurements", measurement);
            response.EnsureSuccessStatusCode();
        }
    }
}