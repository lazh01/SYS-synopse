using DataProcessing.Ingestion.Application.Interfaces;
using DataProcessing.Ingestion.Application.Services;
using DataProcessing.Monitoring;

_ = MonitorService.ServiceName;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IProcessingClient, ProcessingClient>(client =>
{
    // Base address of your Processing API
    client.BaseAddress = new Uri(builder.Configuration["ProcessingService__Url"] ?? "http://processing:80");
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();