using Evently.Modules.Ticketing.Domain.Orders;
using Tickets.Domain.Events;
using Tickets.Domain.Orders;
using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Tickets;

public sealed class Ticket : Entity
{
    public Guid CustomerId { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid EventId { get; private set; }

    public Guid TicketTypeId { get; private set; }

    public string Code { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public bool Archived { get; private set; }


    public static Ticket Create(Order order, TicketType ticketType)
    {
        Ticket ticket = new Ticket(
            order.CustomerId,
            order.Id,
            ticketType.EventId,
            ticketType.Id,
            $"tc_{Guid.CreateVersion7()}",
            DateTime.UtcNow,
            false);

        ticket.RaiseDomainEvent(new TicketCreatedDomainEvent(ticket.Id));

        return ticket;
    }

    public void Archive()
    {
        if (Archived)
            return;

        Archived = true;

        RaiseDomainEvent(new TicketArchivedDomainEvent(Id, Code));
    }
    
    private Ticket(Guid customerId, 
        Guid orderId, 
        Guid eventId, 
        Guid ticketTypeId, 
        string code, 
        DateTime createdAtUtc, 
        bool archived,
        Guid? id = null): base(id)
    {
        CustomerId = customerId;
        OrderId = orderId;
        EventId = eventId;
        TicketTypeId = ticketTypeId;
        Code = code;
        CreatedAtUtc = createdAtUtc;
        Archived = archived;
    }
    
    private Ticket() { }
}