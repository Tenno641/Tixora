using ErrorOr;
using Events.Api.Common;
using Events.Api.Common.Validation;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Events;

public static class PublishEvent
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events/{eventId:guid}/publish", async (Guid eventId, ISender sender) =>
        {
            PublishEventCommand command = new PublishEventCommand(eventId);

            ErrorOr<Success> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok();
        })
        .WithTags(Tags.Events)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}