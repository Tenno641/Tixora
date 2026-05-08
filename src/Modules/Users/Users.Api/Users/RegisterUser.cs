using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Users.Application.Users.RegisterUser;
using ErrorOr;
using Tixora.Shared.Api.Common.Validation;

namespace Users.Api.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (RegisterUserRequest request, ISender sender) =>
        {
            RegisterUserCommand command = new RegisterUserCommand(request.Email, request.Password, request.FirstName, request.LastName);

            ErrorOr<Guid> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.CreatedAtRoute("GetUser", new { Id = result.Value }, result.Value);
        })
        .AllowAnonymous()
        .Produces<Guid>()
        .WithTags(Tags.Users);
    }

}

public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password);
