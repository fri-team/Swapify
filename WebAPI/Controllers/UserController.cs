using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.UserModels;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(ILogger<UserController> logger, IUserService userService, IEmailService emailService)
        {
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel body)
        {
            body.Email = body.Email.ToLower();
            User user = new User(body.Email, body.Name, body.Surname);
            var result = await _userService.AddUserAsync(user, body.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {body.Email} created.");
                string token = await _userService.GenerateEmailConfirmationTokenAsync(user);
                string callbackUrl = Url.Action("ConfirmEmail", "User",
                  new { email = body.Email, token }, protocol: HttpContext.Request.Scheme);
                _emailService.SendRegistrationConfirmationEmail(body.Email, callbackUrl);
                _logger.LogInformation($"Confirmation email to user {body.Email} sent.");
                return Ok();
            }

            StringBuilder identityErrorBuilder = result.Errors.Aggregate(
                            new StringBuilder($"Error when creating user {body.Email}. Identity errors: "),
                            (sb, x) => sb.Append($"{x.Description} "));
            _logger.LogError(identityErrorBuilder.ToString());
            Dictionary<string, string[]> identityErrors = result.Errors.ToDictionary(x => x.Code, x => new string[] { x.Description });
            return ValidationError(identityErrors);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userService.GetUserAsync(email);
            if (user == null)
            {
                _logger.LogError($"Invalid email confirmation attempt. User {email} doesn't exist.");
                return BadRequest();
            }

            if (user.EmailConfirmed)
                return Ok();

            var emailConfirmation = _userService.ConfirmEmailAsync(user, token);
            if (emailConfirmation.Result.Succeeded)
            {
                _logger.LogInformation($"User {email} confirmed email address.");
                return Ok();
            }
            StringBuilder errors = emailConfirmation.Result.Errors.Aggregate(
                           new StringBuilder($"Confirmation of email address {email} failed. Errors: "),
                           (sb, x) => sb.Append($"{x.Description} "));
            _logger.LogError(errors.ToString());
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel body)
        {
            body.Login = body.Login.ToLower();
            var user = await _userService.GetUserAsync(body.Login);
            if (user == null)
            {
                _logger.LogError($"Invalid login attemp. User {body.Login} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Login} neexistuje.");
            }

            var token = await _userService.Authenticate(body.Login, body.Password);
            if(token == null)
                return ErrorResponse("Zadané údaje nie sú správne.");

            var authUser = new AuthenticatedUserModel(token);
            return Ok(authUser);
        }
    }
}
