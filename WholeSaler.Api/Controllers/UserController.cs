using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WholeSaler.Api.DTOs;
using WholeSaler.Api.Models.Jwt;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceWithRedis _userService;
        private readonly JwtSetting _jwtSetting;

        public UserController(IUserServiceWithRedis userService, IOptions<JwtSetting> jwtSetting)
        {
            _userService = userService;
            _jwtSetting = jwtSetting.Value;
        }
        //[HttpPost("login")]
        //public async Task<IActionResult> Login(UserLoginDTO user)
        //{
        //    var result = await _userService.AuthenticateUser(user.Username,user.Password);
        //    return Ok(result);
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            var result = await _userService.AuthenticateUser(user.Username, user.Password);
            if (result == null)
            {
                return BadRequest(result);
            }
            var token = GenerateNewToken(result);
            return Ok(token);
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            await _userService.Create(user);
            return Ok(user);

        }

        private string GenerateNewToken(User vm)
        {
            if (_jwtSetting.Key == null) { throw new Exception("Jwt settings can not be null"); }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claimArray = new List<Claim>
    {
        new Claim(ClaimTypes.Name, vm.Username!),
        new Claim(ClaimTypes.Email, vm.Email!),
        new Claim(ClaimTypes.NameIdentifier, vm.Id!)
    };

          
            //if (vm.Roles != null && vm.Roles.Any())
            //{
            //    foreach (var role in vm.Roles)
            //    {
            //        claimArray.Add(new Claim(ClaimTypes.Role, role));
            //    }
            //}

            if (vm.StoreId != null)
            {
                claimArray.Add(new Claim(ClaimTypes.GroupSid, vm.StoreId));
            }

            var token = new JwtSecurityToken(_jwtSetting.Issuer, _jwtSetting.Audience,
                claimArray,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
