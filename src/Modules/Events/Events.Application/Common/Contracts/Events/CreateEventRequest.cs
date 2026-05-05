namespace Events.Application.Common.Contracts.Events;

public record CreateEventRequest(string Title, string Description, string Location, DateTime StartAt, DateTime EndAt, Guid CategoryId);
