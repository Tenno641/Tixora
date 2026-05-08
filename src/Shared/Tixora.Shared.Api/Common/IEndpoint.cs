using Microsoft.AspNetCore.Routing;

namespace Tixora.Shared.Api.Common;

public interface IEndpoint
{
    void AddEndpoint(IEndpointRouteBuilder app);
}