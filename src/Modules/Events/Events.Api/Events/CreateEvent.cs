using ErrorOr;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;

namespace Events.Api.Events;

public record CreateEventRequest(string Title, string Description, string Location, DateTime StartAt, DateTime EndAt, Guid CategoryId);

internal sealed class CreateEvent: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async ([FromBody] CreateEventRequest createEventRequest, 
                [FromServices] ISender sender) =>
            {
                CreateEventCommand command = new CreateEventCommand(
                CategoryId: createEventRequest.CategoryId,
                Title: createEventRequest.Title,
                Description: createEventRequest.Description,
                Location: createEventRequest.Location,
                StartAt: createEventRequest.StartAt,
                EndAt: createEventRequest.EndAt);

                ErrorOr<Guid> result = await sender.Send(command);

                return result.IsError
                    ? result.ToProblemDetails()
                    : Results.CreatedAtRoute("GetEvent", new { id = result.Value}, result.Value);

        })
        .WithTags(Tags.Events)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
