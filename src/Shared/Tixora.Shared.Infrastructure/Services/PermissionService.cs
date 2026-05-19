using System.Data.Common;
using Dapper;
using ErrorOr;
using Tixora.Shared.Application.Common;

namespace Tixora.Shared.Infrastructure.Services;

public class PermissionService: IPermissionService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public PermissionService(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
                           SELECT 
                               u."Id" AS UserId,
                               pr."PermissionCode" AS Permission
                           FROM users."Users" u
                           JOIN users."RoleUser" ru ON ru."UserId" = u."Id"
                           JOIN users."PermissionRole" pr ON pr."RoleName" = ru."RolesName"
                           WHERE u."IdentityId" = @IdentityId
                           """;

        List<UserPermission> userPermissions = (await connection.QueryAsync<UserPermission>(sql, new { IdentityId = identityId })).AsList();

        PermissionsResponse permissionsResponse = new PermissionsResponse(userPermissions[0].UserId, userPermissions.Select(up =>  up.Permission).ToHashSet());

        return permissionsResponse;
    }

    private class UserPermission
    {
        public Guid UserId { get; set; }
        public string Permission { get; set; }
    }
}