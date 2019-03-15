using System.Security.Claims;
using System.Linq;
namespace Friendly.Core
{
    public static class PrincipalExtensions
    {
        public static bool HasScope(this ClaimsPrincipal principal, string scope)
        {
            return (principal?.HasClaim(c => c.Type == "scope") ?? false) &&
                   principal.Claims.Any(c => c.Type == "scope" && c.Value.Split(' ').Contains(scope));
        }

        public static string GetRemoteIdentifier(this ClaimsPrincipal principal)
        {
            return (principal?.Identity as ClaimsIdentity)?
                .FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}