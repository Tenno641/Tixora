using ErrorOr;
using Events.Api.Common;
using Events.Api.Common.Validation;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Events;

public static class RescheduleEvent
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events/{eventId:guid}/reschedule", async (Guid eventId, DateTime startAt, DateTime endAt, ISender sender) =>
            {
                RescheduleEventCommand command = new RescheduleEventCommand(eventId, startAt, endAt);

                ErrorOr<Success> result = await sender.Send(command);

                return result.IsError
                    ? result.ToProblemDetails()
                    : Results.Ok();
            })
            .WithTags(Tags.Events)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}