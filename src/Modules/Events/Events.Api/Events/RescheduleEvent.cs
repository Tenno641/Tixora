using ErrorOr;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;

namespace Events.Api.Events;

internal sealed class RescheduleEvent: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events/{eventId:guid}/reschedule", async (Guid eventId, DateTime startAt, DateTime endAt, ISender sender) =>
        {
            RescheduleEventCommand command = new RescheduleEventCommand(eventId, startAt, endAt);

            ErrorOr<Success> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.NoContent();
        })
        .WithTags(Tags.Events)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}