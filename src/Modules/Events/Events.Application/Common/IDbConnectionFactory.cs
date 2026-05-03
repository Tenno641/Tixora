using System.Data.Common;

namespace Events.Application.Common;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}