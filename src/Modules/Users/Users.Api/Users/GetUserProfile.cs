using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Users.Application.Users.GetUser;
using ErrorOr;
using Tixora.Shared.Api.Common.Validation;

namespace Users.Api.Users;

internal sealed class GetUserProfile : IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{id:guid}/profile", async (Guid id, ISender sender) =>
        {
            GetUserQuery query = new GetUserQuery(id);
            
            ErrorOr<UserResponse> result = await sender.Send(query);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .Produces<UserResponse>()
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetUser")
        .WithTags(Tags.Users);
    }
}
