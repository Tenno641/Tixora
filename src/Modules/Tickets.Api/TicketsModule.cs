using Microsoft.Extensions.DependencyInjection;
using Tickets.Infrastructure;
using Tixora.Shared.Api;

namespace Tickets.Api;

public static class TicketsModule
{
    public static IServiceCollection AddTicketsModule(this IServiceCollection services)
    {
        services.AddInfrastructure();
        services.RegisterEndpoints(AssemblyReference.Assembly);

        return services;
    }
}