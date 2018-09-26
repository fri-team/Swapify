using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(ILogger<UserController> logger, UserManager<User> userManager, SignInManager<User> signInManager,
            IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel body)
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
                StringBuilder identityErrors = new StringBuilder($"Error when creating user {body.Email}. Identity errors: ");
                foreach (var error in result.Errors)
                {
                    identityErrors.Append($"{error.Description} ");
                }
                _logger.LogError(identityErrors.ToString());
                return BadRequest(result.Errors);
            }
            StringBuilder modelStateErrors = new StringBuilder($"Error when creating user {body.Email}. Modelstate errors: ");
            foreach (var modelError in ModelState)
            {
                foreach (var error in modelError.Value.Errors)
                {
                    modelStateErrors.Append($"{error.ErrorMessage} ");
                }
            }
            _logger.LogError(modelStateErrors.ToString());
            return BadRequest();
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

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {email} logged in.");
            }
            _logger.LogInformation($"User {email} - invalid login attempt.");
            return Ok();
        }
    }
}
