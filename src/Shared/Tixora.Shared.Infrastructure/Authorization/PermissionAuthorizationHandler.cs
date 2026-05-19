using Microsoft.AspNetCore.Authorization;
using Tixora.Shared.Infrastructure.Authentication;

namespace Tixora.Shared.Infrastructure.Authorization;

public class PermissionAuthorizationHandler: AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        HashSet<string> userPermissions = context.User.GetPermissions();

        if (userPermissions.Contains(requirement.Permission))
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}