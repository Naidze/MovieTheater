using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieTheater.Helpers;
using MovieTheater.Models;
using MovieTheater.Models.ViewModels;

namespace MovieTheater.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration Configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            Configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> RequestToken(LoginViewModel model)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
            if (!result.Succeeded)
                return null;

            //security key
            string securityKey = Configuration["SecurityKey"];

            //symetric securiyt key
            var symmetricSecuriytKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials 
            var signingCredentials = new SigningCredentials(symmetricSecuriytKey, SecurityAlgorithms.HmacSha256Signature);

            User user = await _userManager.FindByNameAsync(model.UserName);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            //add claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            //add roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //create token
            var token = new JwtSecurityToken(
                    issuer: "naidzinavicius.com",
                    audience: "naidzinavicius.com",
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: signingCredentials,
                    claims: claims
                );

            //return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
