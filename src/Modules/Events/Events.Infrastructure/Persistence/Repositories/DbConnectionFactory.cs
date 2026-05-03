using System.Data.Common;
using Events.Application.Common;
using Npgsql;

namespace Events.Infrastructure.Persistence.Repositories;

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