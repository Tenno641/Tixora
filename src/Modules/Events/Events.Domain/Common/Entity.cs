namespace Events.Domain.Common;

public class Entity
{
    public Guid Id { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(Guid? id = null)
    {
        Id = id ?? Guid.CreateVersion7();
    }

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public List<IDomainEvent> PopDomainEvents()
    {
        List<IDomainEvent> temp = _domainEvents.ToList();
        _domainEvents.Clear();
        return temp;
    }
    
    private Entity() { }
}