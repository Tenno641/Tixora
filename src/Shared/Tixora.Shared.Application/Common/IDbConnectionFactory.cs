using System.Data.Common;

namespace Tixora.Shared.Application.Common;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}