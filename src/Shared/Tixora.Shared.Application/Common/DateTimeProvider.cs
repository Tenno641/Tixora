using Tixora.Shared.Domain.Common;

namespace Tixora.Shared.Application.Common;

public class DateTimeProvider: IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}