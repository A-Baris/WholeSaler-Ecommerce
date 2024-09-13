using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WholeSaler.Api.Controllers.Base;
using WholeSaler.Api.DTOs;
using WholeSaler.Api.DTOs.User;
using WholeSaler.Api.Models.Jwt;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.TokenServices.Abstract;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.MongoIdentity;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserServiceWithRedis _userService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly JwtSetting _jwtSetting;
        private const string controllerName = "UserController";

        public UserController(IUserServiceWithRedis userService, IOptions<JwtSetting> jwtSetting,ITokenService tokenService,UserManager<AppUser> userManager,ILogger<UserController> logger)
        {
            _userService = userService;
         _tokenService = tokenService;
           _userManager = userManager;
            _logger = logger;
            _jwtSetting = jwtSetting.Value;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return Ok(user);
        }

        [HttpPut("edit/{userId}")]
        public async Task<IActionResult> Edit(Adress adress,string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user.Addresses == null)
            {
                user.Addresses = new List<Adress>();
            }
            user.Addresses.Add(adress);
            var result = await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDto)
        {
            try
            {
                return await ValidateAndExecute(loginDto,
                    async (login) => await _userManager.FindByEmailAsync(login.Email),
                    async user =>
                    {
                        if (user == null) { return BadRequest(); }
                        var userTokens = await _tokenService.CreateToken(user);
                        return userTokens!=null ? Ok(userTokens) : BadRequest();
                    });
;            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Login - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Login - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Create(RegisterDto registerDto)
        {
            try
            {
                var userEmailCheck = await _userManager.FindByEmailAsync(registerDto.Email);
                if (userEmailCheck != null) { return BadRequest("Email is already existing by a user"); }
                var userUsernameCheck = await _userManager.FindByNameAsync(registerDto.Username);
                if (userUsernameCheck != null) { return BadRequest("Username is already existing by a user"); }
                AppUser user = new AppUser()
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone
                };
                var result = await _userManager.CreateAsync(user,registerDto.Password);
                 
                if (result.Succeeded) 
                {

                    return Ok("Register is successful");
                }
                else
                {
                    return BadRequest();
                }


            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Create - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Create - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
 

        }

        [HttpGet("refresh-token/{refreshToken}")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                return await ValidateAndExecute(refreshToken,
                    async (rt) => await _tokenService.RequestTokenByRefresh(rt),
                    result =>
                    {
                        if (result == null)
                        {
                            return Unauthorized("Invalid or expired refresh token.");
                        }

                        return Ok(result);

                    });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} RefreshToken - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} RefreshToken - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }

        
        }
    }
}
