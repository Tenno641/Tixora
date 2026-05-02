namespace Events.Api.Common;

public record Response(Guid Id, string Title, string Description, string Location, DateTime StartAt, DateTime EndAt, string State);
