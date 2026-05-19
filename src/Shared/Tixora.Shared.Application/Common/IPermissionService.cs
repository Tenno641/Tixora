using ErrorOr;

namespace Tixora.Shared.Application.Common;

public interface IPermissionService
{
    Task<ErrorOr<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}

public record PermissionsResponse(Guid UserId, HashSet<string> Permissions);
