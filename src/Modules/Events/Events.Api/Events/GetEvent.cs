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

internal sealed class GetEvent: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, [FromServices] ISender sender) =>
        {
            GetEventQuery query = new GetEventQuery(id);

            ErrorOr<EventTicketResponse> result = await sender.Send(query);

            return result.IsError 
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .WithName("GetEvent")
        .WithTags(Tags.Events)
        .Produces(StatusCodes.Status200OK, typeof(EventResponse))
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
