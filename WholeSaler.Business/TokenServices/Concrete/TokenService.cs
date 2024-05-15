using Amazon.Runtime;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.TokenServices.Abstract;
using WholeSaler.Business.TokenServices.Models;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.MongoIdentity;


namespace WholeSaler.Business.TokenServices.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly CustomTokenOption _tokenOption;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRefreshTokenServiceWithRedis _refreshTokenService;
        private readonly RoleManager<AppRole> _roleManager;

        public TokenService(IOptions<CustomTokenOption> options,UserManager<AppUser> userManager,IRefreshTokenServiceWithRedis refreshTokenService,RoleManager<AppRole> roleManager)
        {
            _tokenOption = options.Value;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
          _roleManager = roleManager;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }

        private IEnumerable<Claim> GetClaims(AppUser appUser, string audience)
        {
            // Create a list of claims for the user
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, appUser.UserName),
        new Claim(ClaimTypes.Email, appUser.Email),
        new Claim(ClaimTypes.Name, appUser.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Aud, audience)
    };
            var roleClaims = new List<Claim>();
            var roles =  _roleManager.Roles.ToList();

            if (appUser.Roles != null && appUser.Roles.Any())
            {
                foreach (var role in roles)
                {
                    foreach (var userRole in appUser.Roles)
                    {
                        if(role.Id==userRole)
                        {
                            roleClaims.Add(new Claim(ClaimTypes.Role, role.Name));
                            break;
                        }
                        
                    }
                }
              
            }
         
            claims.AddRange(roleClaims);

            return claims;
        }
        public async Task<TokenModel> CreateToken(AppUser appUser)
        {
            
            var accesTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);
       
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
               issuer: _tokenOption.Issuer,
               expires: accesTokenExpiration,
               notBefore: DateTime.Now,
               claims: GetClaims(appUser, _tokenOption.Audience),
               signingCredentials: signingCredentials);

            var handleToken = new JwtSecurityTokenHandler();
            


            var token = handleToken.WriteToken(jwtSecurityToken);

       
            var tokenDto = new TokenModel
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accesTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration,
            };
            RefreshToken newRefreshToken = new RefreshToken()
            {
                Token = tokenDto.RefreshToken,
                UserId = appUser.Id.ToString(),
                Expires = tokenDto.RefreshTokenExpiration
            };
            var refreshToken = await _refreshTokenService.FindByUserId(appUser.Id.ToString());
            if (refreshToken == null) { await _refreshTokenService.Create(newRefreshToken);}
            else 
            {
                newRefreshToken.Id = refreshToken.Id;
                await _refreshTokenService.Update(refreshToken.Id, newRefreshToken); 
            }
            
            return tokenDto;
        }

        public async Task<TokenModel> RequestTokenByRefresh(string token)
        {
            var refreshToken = await _refreshTokenService.FindByToken(token);

            if (refreshToken == null)
                return null;

            var user = await _userManager.FindByIdAsync(refreshToken.UserId);
            if (user == null)
                return null;
            TokenModel newTokens = await CreateToken(user);
            refreshToken.Token = newTokens.RefreshToken;
            refreshToken.Expires = newTokens.RefreshTokenExpiration;
            refreshToken.CreatedDate = DateTime.UtcNow;
            await _refreshTokenService.Update(refreshToken.Id, refreshToken);

            return newTokens;
            

        }


    }
}
