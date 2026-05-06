using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Tixora.Shared.Application.Common;
using Tixora.Shared.Infrastructure.Persistence;

namespace Tixora.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructureSharedConfiguration(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.AddSingleton(new NpgsqlDataSourceBuilder(connectionString).Build());

        return services;
    }
}