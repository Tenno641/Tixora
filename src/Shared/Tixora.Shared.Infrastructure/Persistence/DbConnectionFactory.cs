using System.Data.Common;
using Npgsql;
using Tixora.Shared.Application.Common;

namespace Tixora.Shared.Infrastructure.Persistence;

public class DbConnectionFactory: IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;
    
    public DbConnectionFactory(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await _dataSource.OpenConnectionAsync();
    }
}