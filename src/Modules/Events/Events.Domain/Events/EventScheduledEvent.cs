using Events.Domain.Common;

namespace Events.Domain.Events;

public class EventScheduledEvent(Guid Id): DomainEvent;