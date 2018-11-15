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
            _logger.LogInformation(identityErrorBuilder.ToString());
            Dictionary<string, string[]> identityErrors = result.Errors.ToDictionary(x => x.Code, x => new string[] { x.Description });
            return ValidationError(identityErrors);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userService.GetUserAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"Invalid email confirmation attempt. User {email} doesn't exist.");
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
            _logger.LogWarning(errors.ToString());
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel body)
        {
            body.Email = body.Email.ToLower();
            var user = await _userService.GetUserAsync(body.Email);
            if (user == null)
            {
                _logger.LogInformation($"Invalid login attemp. User {body.Email} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Email} neexistuje.");
            }

            if(!user.EmailConfirmed)
            {
                _logger.LogInformation($"Invalid login attemp. User {body.Email} didn't confirm email address.");
                return ErrorResponse($"Pre prihlásenie prosím potvrďte svoju emailovú adresu.");
            }

            var token = await _userService.Authenticate(body.Email, body.Password);
            if(token == null)
            {
                _logger.LogWarning($"Invalid login attemp. User {body.Email} entered wrong password.");
                return ErrorResponse("Zadané heslo nie je správne.");
            }                

            var authUser = new AuthenticatedUserModel(user, token);
            return Ok(authUser);
        }

        [HttpPost("renew")]
        public IActionResult Renew([FromBody] RenewModel body)
        {
            var token = _userService.Renew(body.Token);
            var authUser = new AuthenticatedUserModel(token);
            return Ok(authUser);
        }
    }
}
