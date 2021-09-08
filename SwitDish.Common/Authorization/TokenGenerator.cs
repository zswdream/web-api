using AutoMapper.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Common.Authorization
{
    public static class TokenGenerator
    {
        public static string GenerateJSONWebToken(ApplicationUser user, string secretKey, string issuer, string audience, double tokenValidity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.GivenName, $"{user.FirstName} {user.LastName}")
            };

            //foreach (var claim in user.Claims)
            //{
            //    claims.Add(new System.Security.Claims.Claim(claim.ClaimType, claim.ClaimValue));
            //}

            //foreach (var role in user.Roles)
            //{
            //    claims.Add(new System.Security.Claims.Claim("Role", role.Role.Name));
            //}

            var token = new JwtSecurityToken(
              issuer,
              "https://localhost:44319",
              claims,
              notBefore: DateTime.Now,
              expires: DateTime.Now.AddMinutes(tokenValidity),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
