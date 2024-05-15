using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WholeSaler.Api.DTOs;
using WholeSaler.Api.Models.Jwt;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.TokenServices.Abstract;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.MongoIdentity;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceWithRedis _userService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSetting _jwtSetting;

        public UserController(IUserServiceWithRedis userService, IOptions<JwtSetting> jwtSetting,ITokenService tokenService,UserManager<AppUser> userManager)
        {
            _userService = userService;
         _tokenService = tokenService;
           _userManager = userManager;
            _jwtSetting = jwtSetting.Value;
        }
        //[HttpPost("login")]
        //public async Task<IActionResult> Login(UserLoginDTO user)
        //{
        //    var result = await _userService.AuthenticateUser(user.Username,user.Password);
        //    return Ok(result);
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDto)
        {
           
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) { return BadRequest(); }
           var userTokens = await _tokenService.CreateToken(user);

           
            return Ok(userTokens);
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            await _userService.Create(user);
            return Ok(user);

        }

        [HttpGet("refresh-token/{refreshToken}")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var response = await _tokenService.RequestTokenByRefresh(refreshToken);
            if (response == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            return Ok(response);
        }
    }
}
