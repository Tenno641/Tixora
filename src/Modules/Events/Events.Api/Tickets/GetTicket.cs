using ErrorOr;
using Events.Api.Common;
using Events.Api.Common.Validation;
using Events.Application.Tickets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Tickets;

public static class GetTicket
{
    public static void AddEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("events/{eventId:guid}/tickets/{ticketId:guid}", async (Guid ticketId, Guid eventId, ISender sender) =>
        {
            GetTicketQuery query = new GetTicketQuery(ticketId, eventId);
            
            ErrorOr<TicketResponse> ticket= await sender.Send(query);

            return ticket.IsError
                ? ticket.ToProblemDetails()
                : Results.Ok(ticket.Value);
        })
        .WithTags(Tags.Tickets)
        .Produces<TicketResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("GetTicket");
    }
}