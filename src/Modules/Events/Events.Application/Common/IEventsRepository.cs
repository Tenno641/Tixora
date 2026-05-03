namespace Events.Application.Common;

using Domain.Events;

public interface IEventsRepository
{
    void Insert(Event @event);
}
