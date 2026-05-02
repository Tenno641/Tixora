namespace Events.Api;

using Common;
using Endpoints;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

public static class EventModule
{
    public static IServiceCollection AddEventDependencies(this IServiceCollection services)
    {
        services.AddDbContext<EventsDbContext>(optins =>
        {
            optins.UseNpgsql(Environment.GetEnvironmentVariable("DatabaseConnectionString"), postgresOptions =>
            {
                postgresOptions.MigrationsHistoryTable("Events_Migrations_History", Schema.Events);
            });
        });

        return services;
    }

    public static IEndpointRouteBuilder AddEventEndpoints(this IEndpointRouteBuilder app)
    {
        CreateEvent.AddEndpoint(app);
        GetEvent.AddEndpoint(app);

        return app;
    }
}
