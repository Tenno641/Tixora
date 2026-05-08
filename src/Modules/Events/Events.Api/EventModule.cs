using Events.Application;
using Events.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Api;

namespace Events.Api;

public static class EventModule
{
    public static IServiceCollection AddEventModule(this IServiceCollection services, string connectionString)
    {
        services.AddInfrastructure(connectionString);
        services.RegisterEndpoints(AssemblyReference.Assembly);
        
        return services;
    }
}
