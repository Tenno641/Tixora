using Microsoft.AspNetCore.Routing;

namespace Tixora.Shared.Presentation.Common;

public interface IEndpoint
{
    void AddEndpoint(IEndpointRouteBuilder app);
}