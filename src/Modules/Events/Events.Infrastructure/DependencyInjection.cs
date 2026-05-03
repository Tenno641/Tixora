using Events.Application.Common;
using Events.Infrastructure.Persistence;
using Events.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddPersistence()
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<EventsDbContext>(optins =>
        {
            // optins.UseNpgsql(Environment.GetEnvironmentVariable("DatabaseConnectionString"), postgresOptions =>
            optins.UseNpgsql("Server=localhost; Port=5432; Username=postgres; Password=password; Database=Tixora;", postgresOptions =>
            {
                postgresOptions.MigrationsHistoryTable("Events_Migrations_History", Schema.Events);
            });
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventsRepository, EventRepository>();
        
        return services;
    }
}
