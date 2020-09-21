using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
namespace Api.Providers
{
    public static class RetrieveTokken
    {

        public static int GetIdUtilisateur(this HttpContext context)
        {
            return int.Parse(RetrieveTokken.GetClaim("unique_name", context));
        }

        public static int GetIdRegion(this HttpContext context)
        {
            return int.Parse(RetrieveTokken.GetClaim("idRegion", context));
        }

        public static string GetRoleUtilisateur(this HttpContext context)
        {
            return RetrieveTokken.GetClaim("role", context);
        }

        public static int GetPlaceUtilisateur(this HttpContext context)
        {
            return int.Parse(RetrieveTokken.GetClaim("idPlace", context));
        }

        public static int GetRulePlaceUtilisateur(this HttpContext context)
        {
            string t = RetrieveTokken.GetClaim("idRulePlace", context);
            return int.Parse(t != null ? t : 0.ToString());
        }

        private static string GetClaim(string type, HttpContext context)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // var token0 = await context.GetTokenAsync("name");
            // var me = context.Utilisateur.Claims;
            string tokenString = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            JwtSecurityToken  token = tokenHandler.ReadJwtToken(tokenString);
            Claim claim = token.Claims.SingleOrDefault(e => e.Type == type);

            return claim?.Value;
        }



    }
}