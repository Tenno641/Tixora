namespace Events.Domain.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}