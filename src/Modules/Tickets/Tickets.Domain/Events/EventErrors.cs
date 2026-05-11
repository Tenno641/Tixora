using ErrorOr;

namespace Tickets.Domain.Events;

public static class EventErrors
{
    public static Error NotFound(Guid eventId) =>
        Error.NotFound("Events.Get", $"The event with the identifier {eventId} was not found");

    public readonly static Error StartDateInPast = 
        Error.Conflict("Events.Reschedule", "The event start date is in the past");
}
