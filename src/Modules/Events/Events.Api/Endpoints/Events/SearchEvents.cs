using System.ComponentModel;
using Events.Api.Common;
using Events.Api.Contracts;
using Events.Api.Contracts.Events;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Endpoints.Events;

public static class SearchEvents
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async (string? title, DateTime? startAt, DateTime? endAt, [DefaultValue(15)] int pageSize, [DefaultValue(1)] int page, 
            [FromServices] ISender sender) =>
        {
            SearchEventsQuery query = new SearchEventsQuery(title, startAt, endAt, pageSize, page);

            SearchEventsResponse response = await sender.Send(query);

            return response;
        })
        .WithTags(Tags.Events)
        .Produces<List<EventResponse>>();
    }
}