using Events.Application.Tickets;
using Events.Domain.Tickets;

namespace Events.Application.Common.Mappings;

public static class TicketMappings
{
    public static TicketResponse ToResponse(this TicketType ticketType)
    {
        TicketResponse response = new TicketResponse(ticketType.Id, ticketType.EventId, ticketType.Name, ticketType.Currency, ticketType.Price, ticketType.Quantity);
        
        return response;
    }
}