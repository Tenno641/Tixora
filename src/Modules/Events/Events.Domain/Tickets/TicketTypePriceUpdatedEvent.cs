using Tixora.Shared.Domain.Common;

namespace Events.Domain.Tickets;

public class TicketTypePriceUpdatedEvent(Guid TicketId, decimal newPrice): DomainEvent;