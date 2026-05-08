using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;
using Users.Application.Users.UpdateUser;

namespace Users.Api.Users;

internal sealed class UpdateUserProfile : IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/{id:guid}/profile", async (Guid id, UpdateUserRequest updateUserRequest,  ISender sender) =>
        {
            UpdateUserCommand command = new UpdateUserCommand(id, updateUserRequest.FirstName, updateUserRequest.LastName);

            ErrorOr<Success> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.NoContent();
        })
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent)
        .WithTags(Tags.Users);
    }

}

public record UpdateUserRequest(string FirstName, string LastName);