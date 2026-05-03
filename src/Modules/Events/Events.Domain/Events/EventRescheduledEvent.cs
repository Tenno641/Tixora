using Events.Domain.Common;

namespace Events.Domain.Events;

public class EventRescheduledEvent(Guid Id, DateTime StartAt, DateTime EndAt): DomainEvent;