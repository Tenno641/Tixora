using ErrorOr;
using Events.Domain.Categories;
using Events.Domain.Common;

namespace Events.Domain.Events;

public sealed class Event: Entity
{
    public string Title { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Description { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public string Location { get; private set; }
    public EventState State { get; private set; }

    private Event(
        string title, 
        string description, 
        DateTime startAt, 
        DateTime endAt, 
        string location, 
        EventState state,
        Guid categoryId,
        Guid? id = null): base(id) 
    {
        Title = title;
        Description = description;
        StartAt = startAt;
        EndAt = endAt;
        Location = location;
        State = state;
        CategoryId = categoryId;
    }

    public static ErrorOr<Event> Create(
        string title, 
        string description, 
        DateTime startAt, 
        DateTime endAt, 
        string location, 
        EventState state,
        Category category,
        Guid? id = null)
    {
        if (startAt > endAt)
            return EventErrors.EndDatePrecedesStartDate;
                
        Event @event = new Event(title, description, startAt, endAt, location, state, category.Id, id);
        
        @event.RaiseDomainEvent(new EventCreatedEvent(@event.Id));
        
        return @event;
    }
    
    public ErrorOr<Success> Publish()
    {
        if (State is not EventState.Draft)
            return EventErrors.EventIsNotDraft;

        State = EventState.Published;
        
        RaiseDomainEvent(new EventScheduledEvent(Id));

        return Result.Success;
    }

    public void Reschedule(DateTime startAt, DateTime endAt)
    {
        if (startAt == StartAt && endAt == EndAt)
            return;
        
        StartAt = startAt;
        EndAt = endAt;
        
        RaiseDomainEvent(new EventRescheduledEvent(Id, StartAt, EndAt));
    }
    
    public ErrorOr<Success> Cancel(IDateTimeProvider dateTimeProvider)
    {
        if (State == EventState.Cancelled)
            return EventErrors.EventAlreadyCancelled;

        if (StartAt > dateTimeProvider.UtcNow)
            return EventErrors.EventAlreadyStarted;

        State = EventState.Cancelled;
        
        RaiseDomainEvent(new EventCancelledEvent(Id));

        return Result.Success;
    }
    
    private Event() { }
}