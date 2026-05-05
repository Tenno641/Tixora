using Events.Api.Common;
using Events.Application.Common.Contracts.Events;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Events;

public static class SearchEvents
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async (string? title, DateTime? startAt, DateTime? endAt, int? pageSize, int? page, 
            [FromServices] ISender sender) =>
        {
            SearchEventsQuery query = new SearchEventsQuery(title, startAt, endAt, pageSize ?? 15, page ?? 1);

            SearchEventsResponse response = await sender.Send(query);

            return response;
        })
        .WithTags(Tags.Events)
        .Produces<List<EventResponse>>();
    }
}