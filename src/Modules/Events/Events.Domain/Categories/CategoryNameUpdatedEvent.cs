using Tixora.Shared.Domain.Common;

namespace Events.Domain.Categories;

public class CategoryNameUpdatedEvent(Guid Id, string newName): DomainEvent;