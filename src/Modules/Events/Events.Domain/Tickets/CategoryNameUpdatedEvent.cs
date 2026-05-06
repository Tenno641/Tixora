using Tixora.Shared.Domain.Common;

namespace Events.Domain.Tickets;

public class CategoryNameUpdatedEvent(Guid Id, string newName): DomainEvent;