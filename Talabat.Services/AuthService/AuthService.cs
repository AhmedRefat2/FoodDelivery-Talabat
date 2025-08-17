using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            #region JWT

            /// JWT : Json Web Token
            /// Used For Authentication and Authorization in Web Applications.
            /// Usded For Information Exchange between Backend and Consumer 
            /// Include 3 Parts 

            /// 1. Header 
            /// Security Algrothim signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)

            /// 2.Payload 
            /// Claims : registered, Private and public Claims. 
            /// registered : Predefined Claims like : Issuer, Audience, Expiration, NotBefore, IssuedAt
            /// private : custom Claims like : UserName, Email, Roles, etc.

            /// 3.Signature 
            /// The Last Step Create Token  
            #endregion

            #region Private Claims

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
            };


            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            // this Will add all roles of the user to the claims
            // to help consumer to know what roles the user has without endpoint [GetRoles]
            // This Called : InformationExchange

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration["JWT:Authkey"]));

            #endregion

            #region registered Claims And Create Token

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDayes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);


            #endregion
        }
    }
}
