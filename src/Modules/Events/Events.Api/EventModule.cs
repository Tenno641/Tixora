using Events.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Api;

namespace Events.Api;

public static class EventModule
{
    public static IServiceCollection AddEventModule(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddInfrastructure(databaseConnectionString);
        services.RegisterEndpoints(AssemblyReference.Assembly);
        
        return services;
    }
}
