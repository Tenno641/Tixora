namespace Events.Application.Common.Contracts.Events;

public record EventResponse(Guid Id, string Title, string Description, string Location, DateTime StartAt, DateTime EndAt, string State);
