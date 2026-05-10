namespace Tixora.Shared.Application.Common.EventBus;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}

public class IntegrationEvent: IIntegrationEvent
{
    public IntegrationEvent(Guid? id = null, DateTime? occurredOn = null)
    {
        Id = id ?? Guid.CreateVersion7();
        OccurredOn = occurredOn ?? DateTime.UtcNow;
    }
    
    public Guid Id { get; }
    public DateTime OccurredOn { get; }
}
