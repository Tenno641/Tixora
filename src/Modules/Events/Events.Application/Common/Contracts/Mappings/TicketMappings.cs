using Events.Application.Tickets;
using Events.Domain.Tickets;

namespace Events.Application.Common.Contracts.Mappings;

public static class TicketMappings
{
    public static TicketResponse ToResponse(this Ticket ticket)
    {
        TicketResponse response = new TicketResponse(ticket.EventId, ticket.Name, ticket.Currency, ticket.Price, ticket.Quantity);
        
        return response;
    }
}