using Events.Domain.Common;

namespace Events.Domain.Events;

public class EventCancelledEvent(Guid Id): DomainEvent;