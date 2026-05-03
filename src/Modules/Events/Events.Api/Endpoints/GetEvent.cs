using Events.Api.Contracts;
using Events.Api.Contracts.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Endpoints;

using Application.Events;
using Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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
