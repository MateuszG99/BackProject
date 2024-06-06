using Application.Interfaces;
using Domain.BusinessModels;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController(IIdentityService service) : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(AppUser user) => Ok(service.Login(user));

        [HttpPost]
        public IActionResult Register(string roleName, [FromBody]AppUser user)
        {
            if (roleName == Domain.Constants.Administrator)
            {
                var result = service.RegisterNewAdmin(user);
                return result ? Ok() : BadRequest();
            }
            else if (roleName == Domain.Constants.Moderator)
            {
                var result = service.RegisterNewModerator(user);
                return result ? Ok() : BadRequest();
            }
            else 
            { 
                return BadRequest("Role does not exists!"); 
            }
        }
    }
}
