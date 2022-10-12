using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols;

namespace IdentityServer4.WsFederation
{
    public class RoleClaimAugmentation: IClaimsTransformation
    {
        public const string KlipAdmin = "KLIP Admin";
        public const string KlipPlanAanvrager = "KLIP Planaanvrager";

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var newPrincipal = new ClaimsPrincipal(new ClaimsIdentity(principal.Identity));

            var klipRole = ConfigurationManager.AppSettings["KlipRole"];
            ((ClaimsIdentity)newPrincipal.Identity).AddClaim(new Claim(JwtClaimTypes.Role, klipRole));

            return Task.FromResult(newPrincipal);
        }
    }
}