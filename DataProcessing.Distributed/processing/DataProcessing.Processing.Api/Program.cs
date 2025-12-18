using DataProcessing.Processing.Infrastructure;
using DataProcessing.Processing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ProcessingDb") ??
    "Server=localhost;Database=ProcessingDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;";
builder.Services.AddInfrastructure(connectionString);

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
