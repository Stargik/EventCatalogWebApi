using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApiUser> userManager;
        private readonly IMapper mapper;
        private readonly JwtSettings jwtSettings;

        public AccountService(UserManager<ApiUser> userManager, IMapper mapper, IOptions<JwtSettings> jwtSettings)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.jwtSettings = jwtSettings.Value;
        }

        public async Task RegisterUser(UserModel userModel)
        {
            var user = mapper.Map<ApiUser>(userModel);
            var result  = await userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                throw new EventCatalogIdentityException();
            }

            await userManager.AddToRolesAsync(user, userModel.Roles);
        }

        public async Task<string> LoginUser(UserLoginModel userLoginModel)
        {
            if (!await ValidateUser(userLoginModel))
            {
                throw new EventCatalogIdentityException();
            }
            var user = await userManager.FindByNameAsync(userLoginModel.Email);
            return await CreateToken(user);
        }

        private async Task<string> CreateToken(ApiUser user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var token = GenerateToken(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<bool> ValidateUser(UserLoginModel userLoginModel)
        {
            var user = await userManager.FindByNameAsync(userLoginModel.Email);
            if (user is not null && await userManager.CheckPasswordAsync(user, userLoginModel.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private JwtSecurityToken GenerateToken(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var expiration = DateTime.Now.AddMinutes(jwtSettings.Lifetime);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );
            return token;
        }

        private async Task<List<Claim>> GetClaims(ApiUser user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}
