using Events.Api.Common;
using Events.Application.Tickets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Tickets;

public static class GetTickets
{
    public static void AddEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("events/{eventId:guid}/tickets", async (Guid eventId, ISender sender) =>
        {
            GetTicketsQuery query = new GetTicketsQuery(eventId);

            List<TicketResponse> tickets = await sender.Send(query);

            return Results.Ok(tickets);
        })
        .WithTags(Tags.Tickets)
        .Produces<List<TicketResponse>>(StatusCodes.Status200OK);
    }
}