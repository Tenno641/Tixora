using MediatR;

namespace Tixora.Shared.Domain.Common;

public interface IDomainEvent: INotification
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}

public class DomainEvent : IDomainEvent
{
    public Guid Id => Guid.CreateVersion7();
    public DateTime OccurredOn => DateTime.UtcNow;
}