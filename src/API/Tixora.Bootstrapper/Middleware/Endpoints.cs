using Tixora.Shared.Presentation.Common;

namespace Tixora.Bootstrapper.Middleware;

public static class Endpoints
{
    public static void MapEndpoints(this IApplicationBuilder app, IEndpointRouteBuilder endpoints)
    {
        IEnumerable<IEndpoint> endpointsTypes = app.ApplicationServices.GetServices<IEndpoint>();
        
        foreach (IEndpoint endpoint in endpointsTypes)
            endpoint.AddEndpoint(endpoints);
    }
}