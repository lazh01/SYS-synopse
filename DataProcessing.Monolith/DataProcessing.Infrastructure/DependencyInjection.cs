using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SensorData.Application.Interfaces;
using SensorData.Application.UseCases;
using SensorData.Infrastructure.Persistence;

namespace SensorData.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddDbContext<SensorDbContext>(options =>
            options.UseInMemoryDatabase("SensorDb"));

        services.AddScoped<ISensorAggregateRepository, SensorAggregateRepository>();
        services.AddScoped<IIngestMeasurementUseCase, IngestMeasurementUseCase>();

        return services;
    }
}
