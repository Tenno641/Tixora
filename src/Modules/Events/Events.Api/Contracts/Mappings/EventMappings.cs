using Events.Domain.Events;

namespace Events.Api.Contracts.Mappings;

public static class EventMappings
{
    public static EventResponse ToResponse(this Event @event)
    {
        var eventResponse = new EventResponse(
        Id: @event.Id,
        Title: @event.Title,
        Description: @event.Description,
        Location: @event.Location,
        StartAt: @event.StartAt,
        EndAt: @event.EndAt,
        State: @event.State.ToString());

        return eventResponse;
    }
}
