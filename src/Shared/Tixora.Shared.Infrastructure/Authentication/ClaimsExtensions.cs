using System.Security.Claims;

namespace Tixora.Shared.Infrastructure.Authentication;

public static class ClaimsExtensions
{
    // TODO: Throw More Specific Exceptions 
    public static string GetId(this ClaimsPrincipal principal) => principal.FindFirstValue(CustomClaims.Sub) ?? throw new Exception();
    public static string GetIdentityId(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();
    public static HashSet<string> GetPermissions(this ClaimsPrincipal principal) => principal.FindAll(CustomClaims.Permission).Select(claim => claim.Value).ToHashSet();
}