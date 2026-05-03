namespace Events.Api.Contracts;

public record CreateEventRequest(string Title, string Description, string Location, DateTime StartAt, DateTime EndAt);
