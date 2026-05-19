using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Tixora.Shared.Infrastructure.Authorization;

public class PermissionAuthorizationPolicyProvider: DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }
    
    public async override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
            return policy;

        AuthorizationPolicy permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
        
        return permissionPolicy;
    }
}