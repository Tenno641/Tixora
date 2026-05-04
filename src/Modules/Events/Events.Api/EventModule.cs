using Events.Api.Endpoints;
using Events.Application;
using Events.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace Events.Api;

public static class EventModule
{
    public static IServiceCollection AddEventDependencies(this IServiceCollection services)
    {
        services.AddInfrastructure();
        services.AddApplication();

        return services;
    }
    
    public static IEndpointRouteBuilder AddEventEndpoints(this IEndpointRouteBuilder app)
    {
        CreateEvent.AddEndpoint(app);
        GetEvent.AddEndpoint(app);
        SearchEvents.AddEndpoint(app);

        return app;
    }
}
