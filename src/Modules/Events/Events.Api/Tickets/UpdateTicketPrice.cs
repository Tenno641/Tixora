using ErrorOr;
using Events.Api.Common;
using Events.Api.Common.Validation;
using Events.Application.Tickets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Tickets;

public static class UpdateTicketPrice
{
    public static void AddEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("events/{eventId:guid}/tickets/{ticketId:guid}", async (Guid ticketId, Guid eventId, decimal newPrice, ISender sender) =>
        {
            UpdateTicketPriceCommand command = new UpdateTicketPriceCommand(eventId, ticketId, newPrice);

            ErrorOr<Guid> ticket = await sender.Send(command);

            return ticket.IsError
                ? ticket.ToProblemDetails()
                : Results.Ok(ticket.Value);
        })
        .WithTags(Tags.Tickets)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}