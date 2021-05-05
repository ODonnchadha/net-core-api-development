using HotelListing.DTOs.Users;
using HotelListing.Interfaces.Managers;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Managers
{
    public class AuthManager : IAuthManager
    {
        private User user;

        private readonly IConfiguration configuration;
        private readonly UserManager<User> manager;
        public AuthManager(IConfiguration configuration, UserManager<User> manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        public string CreateToken()
        {
            var credentials = GetSigningCredentials();
            var claims = GetClaims();
            var token = GenerateToken(credentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateToken(SigningCredentials credentials, List<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(double.Parse(jwtSettings.GetSection("Lifetime").Value));

            var options = new JwtSecurityToken(
                // audience: jwtSettings.GetSection("Issuer").Value,
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return options;
        }

        private List<Claim> GetClaims()
        {
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = manager.GetRolesAsync(user).Result;
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = jwtSettings.GetSection("Key").Value;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Does this user exist within the system and is the password valid?
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public async Task<bool> ValidateUserAsync(UserLogin userLogin)
        {
            user = await manager.FindByNameAsync(userLogin.Email);
            return (user != null && await manager.CheckPasswordAsync(user, userLogin.Password));
        }
    }
}
