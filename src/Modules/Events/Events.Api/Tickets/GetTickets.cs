using Events.Application.Tickets;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ErrorOr;
using Tixora.Shared.Presentation.Common;
using Tixora.Shared.Presentation.Common.Validation;

namespace Events.Api.Tickets;

internal sealed class GetTickets: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{eventId:guid}/tickets", async (Guid eventId, ISender sender) =>
        {
            GetTicketsQuery query = new GetTicketsQuery(eventId);

            ErrorOr<List<TicketResponse>> result = await sender.Send(query);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .WithTags(Tags.Tickets)
        .Produces<List<TicketResponse>>(StatusCodes.Status200OK);
    }
}