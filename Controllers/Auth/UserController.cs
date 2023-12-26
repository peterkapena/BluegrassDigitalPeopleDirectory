using BluegrassDigitalPeopleDirectory.Models;
using BluegrassDigitalPeopleDirectory.Services;
using BluegrassDigitalPeopleDirectory.Services.Bug;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BluegrassDigitalPeopleDirectory.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserManager<User> userMgr,
                          DBContext context,
                          Services.Setting.Setting setting,
                           IUserService userService,
                          IErrorLogService errorLogService) : CommonAPI(userMgr: userMgr,
            context: context,
            setting: setting,
             errorLogService: errorLogService)
    {
        public IUserService UserService { get; } = userService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelIn loginModelIn)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(loginModelIn.Email);
                LoginModelOut loginModelOut = new();

                if (user is null && !await UserManager.CheckPasswordAsync(user, loginModelIn.Password))
                {
                    loginModelOut.AddError("login", "login failed");
                    return Unauthorized(loginModelOut);
                }
                else
                {
                    string token = await UserService.GetAuthToken(user);
                    loginModelOut.Token = token;
                    loginModelOut.Email = user.Email;
                    return Ok(loginModelOut);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelIn registerModelIn)
        {
            var user = await UserManager.FindByEmailAsync(registerModelIn.Email);
            CommonOutputModel registerModelOut = new();

            if (user is null)
            {
                var rslt = await UserService.CreateAsync(registerModelIn);
                if (!rslt.Succeeded)
                {
                    foreach (var error in rslt.Errors)
                    {
                        registerModelOut.AddError(error.Code, error.Description);
                    }
                    return StatusCode(500, registerModelOut);
                }
                return Ok(registerModelOut);
            }
            else
            {
                registerModelOut.AddError("Duplicate", "A user with this email already exists.");

                return StatusCode(500, registerModelOut);
            }
        }
    }
}
