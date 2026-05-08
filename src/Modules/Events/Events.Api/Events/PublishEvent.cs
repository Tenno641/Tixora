using ErrorOr;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;

namespace Events.Api.Events;

internal sealed class PublishEvent: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events/{eventId:guid}/publish", async (Guid eventId, ISender sender) =>
        {
            PublishEventCommand command = new PublishEventCommand(eventId);

            ErrorOr<Success> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.NoContent();
        })
        .WithTags(Tags.Events)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}