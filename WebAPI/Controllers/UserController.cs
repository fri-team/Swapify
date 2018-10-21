using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Identity;
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
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, UserManager<User> userManager, IUserService userService,
            IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _userService = userService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel body)
        {
            if (ModelState.IsValid)
            {
                User user = new User(body.Email, body.Name, body.Surname);
                var result = await _userManager.CreateAsync(user, body.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation(string.Format("User {0} created.", body.Email));
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                      "ConfirmEmail", "User",
                      new { email = body.Email, token },
                      protocol: HttpContext.Request.Scheme);

                    _emailService.SendRegistrationConfirmationEmail(body.Email, callbackUrl);
                    return Ok();
                }
                StringBuilder identityErrorBuilder = result.Errors.Aggregate(
                            new StringBuilder($"Error when creating user {body.Email}. Identity errors: "),
                            (sb, x) => sb.Append($"{x.Description} ")
                        );
                _logger.LogError(identityErrorBuilder.ToString());

                Dictionary<string, string[]> identityErrors = result.Errors.ToDictionary(x => x.Code, x => new string[] { x.Description });
                return ValidationError(identityErrors);
            }
            StringBuilder modelStateBuilder = ModelState.Values.SelectMany(x => x.Errors).Aggregate(
                            new StringBuilder($"Error when creating user {body.Email}. ModelState errors: "),
                            (sb, x) => sb.Append($"{x.ErrorMessage} "));
            _logger.LogError(modelStateBuilder.ToString());

            return ValidationError(ModelState);
        }

        public IActionResult ConfirmEmail(string email, string token)
        {
            User user = _userManager.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                _logger.LogError($"Invalid email confirmation attempt. User {email} doesn't exist.");
                return BadRequest();
            }
            else
            {
                if (user.EmailConfirmed)
                    return Ok();

                var emailConfirmation = _userManager.ConfirmEmailAsync(user, token);
                if (emailConfirmation.Result.Succeeded)
                {
                    _logger.LogInformation($"User {email} confirmed email address.", email);
                    return Ok();
                }

                StringBuilder errors = new StringBuilder($"Confirmation of email address {email} failed. Errors: ");
                foreach (var errorMessage in emailConfirmation.Result.Errors)
                {
                    errors.Append($"{errorMessage.Description} ");
                }
                _logger.LogError(errors.ToString());
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel body)
        {
            if (body == null || !ModelState.IsValid)
                return BadRequest();

            var token = _userService.Authenticate(body.Login, body.Password);
            var authUser = new AuthenticatedUserModel(token);

            return Ok(authUser);
        }
    }
}
