using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using StackExchange.Redis;
using Tixora.Shared.Application.Common;
using Tixora.Shared.Infrastructure.Common;
using Tixora.Shared.Infrastructure.Persistence;

namespace Tixora.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructureSharedConfiguration(this IServiceCollection services, 
        string databaseConnectionString,
        string redisConnectionString)
    {
        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddStackExchangeRedisCache(options => 
        {
            options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
        });
        
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.AddSingleton(new NpgsqlDataSourceBuilder(databaseConnectionString).Build());
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}