using Tixora.Shared.Domain.Common;

namespace Events.Domain.Events;

public class EventCreatedEvent(Guid Id) : DomainEvent;