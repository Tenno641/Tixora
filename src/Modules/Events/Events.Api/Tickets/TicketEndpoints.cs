using Microsoft.AspNetCore.Routing;

namespace Events.Api.Tickets;

public static class TicketEndpoints
{
    public static void AddTicketEndpoints(IEndpointRouteBuilder app)
    {
        CreateTicket.AddEndpoint(app);
        GetTicket.AddEndpoint(app);
        GetTickets.AddEndpoint(app);
    }
}