using Events.Api.Common;
using Events.Api.Common.Validation;
using Events.Api.Contracts;
using Events.Application.Events;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Endpoints;

public static class CreateEvent
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async ([FromBody] CreateEventRequest createEventRequest, 
                [FromServices] ISender sender) =>
            {
                var command = new CreateEventCommand(
                CategoryId: createEventRequest.CategoryId,
                Title: createEventRequest.Title,
                Description: createEventRequest.Description,
                Location: createEventRequest.Location,
                StartAt: createEventRequest.StartAt,
                EndAt: createEventRequest.EndAt);

                var result = await sender.Send(command);

                return result.IsError
                    ? result.Errors.ToValidationProblem()
                    : Results.CreatedAtRoute("GetEvent", new { id = result}, result);

        })
        .WithTags(Tags.Events)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
