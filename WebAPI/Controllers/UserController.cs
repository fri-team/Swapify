using FRITeam.Swapify.Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] CredentialsBody body)
        {
            if (body == null || !body.IsValid())
                return BadRequest();

            var token = _userService.Authenticate(body.Login, body.Password);
            var authUser = new AuthUser(token);

            return Ok(authUser);
        }
    }
}
