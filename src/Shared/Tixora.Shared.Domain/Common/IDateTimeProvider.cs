namespace Tixora.Shared.Domain.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}