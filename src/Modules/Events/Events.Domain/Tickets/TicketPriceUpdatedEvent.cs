using Events.Domain.Common;

namespace Events.Domain.Tickets;

public class TicketPriceUpdatedEvent(Guid TicketId, decimal newPrice): DomainEvent;