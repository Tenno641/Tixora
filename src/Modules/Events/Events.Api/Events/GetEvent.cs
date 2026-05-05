using Events.Api.Common;
using Events.Application.Common.Contracts.Events;
using Events.Application.Common.Contracts.Mappings;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Events;

public static class GetEvent
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, [FromServices] ISender sender) =>
        {
            var query = new GetEventQuery(id);

            var result = await sender.Send(query);

            return result is null
                ? Results.NotFound()
                : Results.Ok(result.ToResponse());
        })
        .WithName("GetEvent")
        .WithTags(Tags.Events)
        .Produces(StatusCodes.Status200OK, typeof(EventResponse))
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
