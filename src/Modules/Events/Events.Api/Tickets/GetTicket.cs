using ErrorOr;
using Events.Application.Tickets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;

namespace Events.Api.Tickets;

internal sealed class GetTicket: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
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