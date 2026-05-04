using ErrorOr;

namespace Events.Domain.Events;

public static class EventErrors
{
    public readonly static Error EndDatePrecedesStartDate = Error.Forbidden("Event.CreateEvent", "End date cannot be before the start date.");
    public readonly static Error EventIsNotDraft = Error.Forbidden("Event.Publish", "Event is not a draft.");
    public readonly static Error EventAlreadyCancelled = Error.Conflict("Event.Cancel", "Event is already cancelled.");
    public readonly static Error EventAlreadyStarted = Error.Conflict("Event.Cancel", "Event already started.");
    public readonly static Error EventIsNotFound = Error.NotFound("Get.Event", "Event is not found");
    public readonly static Error TicketsNotFound = Error.NotFound("Get.Event", "Tickets are not found.");
}