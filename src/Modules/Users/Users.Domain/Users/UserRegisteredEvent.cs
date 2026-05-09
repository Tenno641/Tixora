using Tixora.Shared.Domain.Common;

namespace Users.Domain.Users;

public class UserRegisteredEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; init; } = userId;
}
