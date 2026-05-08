using ErrorOr;
using Events.Application.Tickets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;

namespace Events.Api.Tickets;

public record CreateTicketRequest(string Name, string Currency,  decimal Price, int Quantity);

internal sealed class CreateTicket: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
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