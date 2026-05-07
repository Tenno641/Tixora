using Events.Api.Common;
using Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ErrorOr;
using Events.Api.Common.Validation;

namespace Events.Api.Events;

public static class SearchEvents
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async (string? title, DateTime? startAt, DateTime? endAt, int? pageSize, int? page, 
            [FromServices] ISender sender) =>
        {
            SearchEventsQuery query = new SearchEventsQuery(title, startAt, endAt, pageSize ?? 15, page ?? 1);

            ErrorOr<SearchEventsResponse> result = await sender.Send(query);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .WithTags(Tags.Events)
        .Produces<List<EventResponse>>();
    }
}