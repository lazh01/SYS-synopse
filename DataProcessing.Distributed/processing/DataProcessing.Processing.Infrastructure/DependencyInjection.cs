using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessing.Processing.Application.Interfaces;
using DataProcessing.Processing.Application.UseCases;
using DataProcessing.Processing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataProcessing.Processing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ProcessingDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IMeasurementRepository, MeasurementRepository>();
        services.AddScoped<IngestMeasurementUseCase>();
        services.AddScoped<GetAggregatesUseCase>();

        return services;
    }
}
