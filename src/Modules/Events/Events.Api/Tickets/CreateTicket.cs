using ErrorOr;
using Events.Api.Common;
using Events.Api.Common.Validation;
using Events.Application.Common.Contracts.Tickets;
using Events.Application.Tickets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Tickets;

public static class CreateTicket
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events/{eventId:guid}/tickets", async (Guid eventId, CreateTicketRequest request, [FromServices] ISender sender) =>
        {
            CreateTicketCommand command = new CreateTicketCommand(eventId, request.Name, request.Currency, request.Price, request.Quantity);

            ErrorOr<Guid> ticketCreation = await sender.Send(command);

            return ticketCreation.IsError
                ? ticketCreation.ToProblemDetails()
                : Results.CreatedAtRoute("GetTicket", new { TicketId = ticketCreation.Value, EventId = eventId }, ticketCreation.Value);
        })
        .WithTags(Tags.Tickets)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}