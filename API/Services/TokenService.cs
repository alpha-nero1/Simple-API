using Domain;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace API.Services
{
    // Example use of interface and class use for DI.
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }

    // Actual implementation.
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(AppUser user)
        {
            // These "claims" are items that will be encoded into the token.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Sign token with encrypted key.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
            // Configure how the token will be signed.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            // Setup the token options.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Set the claims vars.
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Return the string.
            return tokenHandler.WriteToken(token);
        }
    }
}
