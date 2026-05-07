using ErrorOr;
using Events.Application.Tickets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Presentation.Common;
using Tixora.Shared.Presentation.Common.Validation;

namespace Events.Api.Tickets;

internal sealed class UpdateTicketPrice
{
    public void AddEndpoint(IEndpointRouteBuilder app)
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