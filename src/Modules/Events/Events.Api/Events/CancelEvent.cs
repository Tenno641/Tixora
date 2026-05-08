using ErrorOr;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;

namespace Events.Api.Events;

internal sealed class CancelEvent: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("event/{eventId:guid}/cancel", async (Guid eventId, ISender sender) =>
        {
            CancelEventCommand command = new CancelEventCommand(eventId);

            ErrorOr<Success> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.NoContent();
        })
        .WithTags(Tags.Events)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status204NoContent);
    }
}