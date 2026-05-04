using Events.Domain.Common;

namespace Events.Application.Common;

public class DateTimeProvider: IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}