using System;
using System.Configuration;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json.Linq;

namespace IdentityServer4.WsFederation
{
    public class RoleClaimAction: ClaimAction
    {
        public RoleClaimAction() : base(string.Empty, string.Empty)
        {
        }

        public override void Run(JObject userData, ClaimsIdentity identity, string issuer)
        {
            var klipRole = ConfigurationManager.AppSettings["KlipRole"];
            identity.AddClaim(new Claim(JwtClaimTypes.Role, klipRole));
        }
    }
}