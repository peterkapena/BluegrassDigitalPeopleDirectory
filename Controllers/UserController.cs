using BluegrassDigitalPeopleDirectory.Models;
using BluegrassDigitalPeopleDirectory.Services;
using BluegrassDigitalPeopleDirectory.Services.Bug;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BluegrassDigitalPeopleDirectory.Controllers.Identity
{
    [Route("[controller]")]
    [ApiController]
    public class UserController(UserManager<User> userMgr,
                          DBContext context,
                          Services.Setting.Setting setting,
                           IUserService userService,
                          IErrorLogService errorLogService) : BaseAPI(userMgr: userMgr,
            context: context,
            setting: setting,
             errorLogService: errorLogService)
    {


        public IUserService UserService { get; } = userService;

        [HttpPost(Name = "login")]
        public async Task<ActionResult> Login([FromBody] XUser xUser)
        {
            var user = await UserManager.FindByEmailAsync(xUser.Email);

            if (user is null && !(await UserManager.CheckPasswordAsync(user, xUser.Password)))
            {
                SetReturnValue("error", "Login failed.");
                return Unauthorized(ReturnValue);
            }
            else
            {
                string token = await UserService.GetAuthToken(user);
                ReturnValue = GetReturnForLogin(user, token);
                return Ok(ReturnValue);
            }
        }

        private static Dictionary<string, object> GetReturnForLogin(User u, string token)
        {
            return new Dictionary<string, object>
            {
                { "id", u.Id },
                { "email", u.Email },
                { "token", token },
             };
        }
    }
}
