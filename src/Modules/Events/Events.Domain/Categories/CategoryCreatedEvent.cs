using Tixora.Shared.Domain.Common;

namespace Events.Domain.Categories;

public class CategoryCreatedEvent(Guid Id): DomainEvent;