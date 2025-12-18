using DataProcessing.Dashboard.Application.Interfaces;
using DataProcessing.Dashboard.Infrastructure.Persistence;
using DataProcessing.Dashboard.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataProcessing.Dashboard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<DashboardDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ISensorAggregateQuery, SensorAggregateQuery>();

        return services;
    }
}