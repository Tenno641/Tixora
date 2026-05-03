namespace Events.Api.Contracts;

public record EventResponse(Guid Id, string Title, string Description, string Location, DateTime StartAt, DateTime EndAt, string State);
