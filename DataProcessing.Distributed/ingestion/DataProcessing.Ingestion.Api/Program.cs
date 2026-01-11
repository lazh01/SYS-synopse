using DataProcessing.Ingestion.Application.Interfaces;
using DataProcessing.Ingestion.Application.Services;
using DataProcessing.Monitoring;
using RabbitMQ.Client;

_ = MonitorService.ServiceName;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HTTP clients for processing service
var processingServiceUrl = builder.Configuration["ProcessingService__Url"] ?? "http://processing:80";
builder.Services.AddHttpClient<IProcessingClient, ProcessingClient>(client =>
{
    client.BaseAddress = new Uri(processingServiceUrl);
});
builder.Services.AddHttpClient<IAsyncProcessingClient, AsyncProcessingClient>(client =>
{
    client.BaseAddress = new Uri(processingServiceUrl);
});

// Register RabbitMQ connection
var rabbitMqHost = builder.Configuration["RabbitMQ__Host"] ?? "rabbitmq";
var rabbitMqPort = int.Parse(builder.Configuration["RabbitMQ__Port"] ?? "5672");
var rabbitMqUsername = builder.Configuration["RabbitMQ__Username"] ?? "guest";
var rabbitMqPassword = builder.Configuration["RabbitMQ__Password"] ?? "guest";

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = rabbitMqHost,
        Port = rabbitMqPort,
        UserName = rabbitMqUsername,
        Password = rabbitMqPassword
    };
    return factory.CreateConnection();
});

builder.Services.AddSingleton<MessageBrokerClient>();
builder.Services.AddSingleton<IMessageBrokerClient>(sp => sp.GetRequiredService<MessageBrokerClient>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();