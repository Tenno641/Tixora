using Events.Api.Categories;
using Events.Api.Events;
using Events.Api.Tickets;
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
        EventEndpoints.AddEndpoints(app);
        TicketEndpoints.AddEndpoints(app);
        CategoryEndpoints.AddEndpoints(app);

        return app;
    }
}
