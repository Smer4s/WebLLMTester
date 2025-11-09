
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Uply.Web.Extensions;


public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal principal)
    {
        var idClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier) ?? throw new Exception("Couldn't get Id");

        if (!Guid.TryParse(idClaim.Value, out var id))
        {
            throw new Exception("Couldn't parse Id");
        }

        return id;
    }
}
