using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Tixora.Shared.Application.Common;
using Tixora.Shared.Infrastructure.Authentication;
using ErrorOr;

namespace Tixora.Shared.Infrastructure.Authorization;

public class CustomClaimsTransformation: IClaimsTransformation
{
    private readonly IPermissionService _permissionService;
    
    public CustomClaimsTransformation(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
    
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type == CustomClaims.Sub))
        {
            // Short circuit as this method may get called multiple times
            // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-10.0#extend-or-add-custom-claims-using-iclaimstransformation
            return principal;
        }

        string identityId = principal.GetIdentityId();
        
        ErrorOr<PermissionsResponse> userPermissionsResult = await _permissionService.GetUserPermissionsAsync(identityId);

        if (userPermissionsResult.IsError)
            throw new Exception(); // TODO: Throw More Specific Exception

        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identityId));
        
        claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, userPermissionsResult.Value.UserId.ToString()));

        foreach (string permission in userPermissionsResult.Value.Permissions)
        {
            claimsIdentity.AddClaim(new Claim(CustomClaims.Permission, permission));
        }
        
        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}