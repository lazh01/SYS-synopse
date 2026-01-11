using DataProcessing.Processing.Infrastructure;
using DataProcessing.Processing.Infrastructure.Persistence;
using DataProcessing.Processing.Api.Services;
using Microsoft.EntityFrameworkCore;
using DataProcessing.Monitoring;
using RabbitMQ.Client;

_ = MonitorService.ServiceName;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ProcessingDb") ??
    "Server=localhost;Database=ProcessingDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;";
builder.Services.AddInfrastructure(connectionString);

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

// Register the message consumer as a hosted service
builder.Services.AddHostedService<MeasurementConsumerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProcessingDbContext>();
    db.Database.Migrate();  // <-- This applies any pending migrations
}

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Using Development environment");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
