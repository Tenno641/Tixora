using ErrorOr;
using Events.Api.Common;
using Events.Api.Common.Validation;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Events;

public static class CancelEvent
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("event/{eventId:guid}/cancel", async (Guid eventId, ISender sender) =>
        {
            CancelEventCommand command = new CancelEventCommand(eventId);

            ErrorOr<Success> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok();
        })
        .WithTags(Tags.Events)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status200OK);
    }
}