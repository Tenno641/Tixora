using Events.Api.Common;
using Events.Api.Contracts;
using Events.Api.Contracts.Mappings;
using Events.Application.Events;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Endpoints;

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
