using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Users.Application.Common;
using Users.Application.Users;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Tixora.Shared.Api.Common.Validation;

namespace Users.Api.Users;

public class LoginUser: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/login", async (LoginUserRequest request, ISender sender) =>
        {
            LoginUserCommand command = new LoginUserCommand(request.Email, request.Password);

            ErrorOr<LoginResponse> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .AllowAnonymous()
        .WithTags(Tags.Users)
        .Produces<LoginResponse>()
        .Produces(StatusCodes.Status401Unauthorized);
    }
}

public record LoginUserRequest(string Email, string Password);