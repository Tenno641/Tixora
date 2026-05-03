using Events.Domain.Common;

namespace Events.Domain.Events;

public sealed class Event: Entity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public string Location { get; private set; }
    public EventState State { get; private set; }
    
    public static Event Create(
        string title, 
        string description, 
        DateTime startAt, 
        DateTime endAt, 
        string location, 
        EventState state, 
        Guid? id = null)
    {
        Event @event = new Event(title, description, startAt, endAt, location, state, id);
        
        @event.RaiseDomainEvent(new EventCreatedEvent(@event.Id));
        
        return @event;
    }

    private Event(
        string title, 
        string description, 
        DateTime startAt, 
        DateTime endAt, 
        string location, 
        EventState state, 
        Guid? id = null): base(id) 
    {
        Title = title;
        Description = description;
        StartAt = startAt;
        EndAt = endAt;
        Location = location;
        State = state;
    }

    private Event() { }
}