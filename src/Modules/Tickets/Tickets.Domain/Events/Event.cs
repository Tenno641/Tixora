using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Events;

public sealed class Event : Entity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Location { get; private set; }
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public bool Canceled { get; private set; }

    public static Event Create(
        Guid id,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        Event @event = new Event(title, description, location, startsAtUtc, endsAtUtc, false, id);

        return @event;
    }

    public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;

        RaiseDomainEvent(new EventRescheduledDomainEvent(Id, StartsAtUtc, EndsAtUtc));
    }

    public void Cancel()
    {
        if (Canceled)
            return;

        Canceled = true;

        RaiseDomainEvent(new EventCanceledDomainEvent(Id));
    }

    public void PaymentsRefunded()
    {
        RaiseDomainEvent(new EventPaymentsRefundedDomainEvent(Id));
    }

    public void TicketsArchived()
    {
        RaiseDomainEvent(new EventTicketsArchivedDomainEvent(Id));
    }

    private Event(string title, string description, string location, DateTime startsAtUtc, DateTime? endsAtUtc, bool canceled, Guid? id = null) : base(id)
    {
        Title = title;
        Description = description;
        Location = location;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        Canceled = canceled;
    }

    private Event() { }
}
