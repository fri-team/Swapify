using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.UserModels;
using FRITeam.Swapify.Backend.Settings;
using Microsoft.Extensions.Options;
using WebAPI.Extensions;
using System.Net;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly Uri _baseUrl;

        public UserController(ILogger<UserController> logger, IUserService userService, IEmailService emailService,
            IOptions<EnvironmentSettings> environmentSettings)
        {
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
            _baseUrl = new Uri(environmentSettings.Value.BaseUrl);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel body)
        {
            body.Email = body.Email.ToLower();
            User user = new User(body.Email, body.Name, body.Surname);
            var addResult = await _userService.AddUserAsync(user, body.Password);
            if (!addResult.Succeeded)
            {
                _logger.LogInformation(ControllerExtensions.IdentityErrorBuilder($"Error when creating user {body.Email}. Identity errors: ", addResult.Errors));
                Dictionary<string, string[]> identityErrors = ControllerExtensions.IdentityErrorsToDictionary(addResult.Errors);
                return ValidationError(identityErrors);
            }

            _logger.LogInformation($"User {body.Email} created.");
            string token = await _userService.GenerateEmailConfirmationTokenAsync(user);
            user = await _userService.GetUserByEmailAsync(body.Email);
            string callbackUrl = new Uri(_baseUrl, $@"confirmEmail/{user.Id}/{token}").ToString();

            if (!_emailService.SendConfirmationEmail(body.Email, callbackUrl, "RegistrationEmail"))
            {
                _logger.LogError($"Error when sending confirmation email to user {body.Email}.");
                var deleteResult = await _userService.DeleteUserAsyc(user);
                if (deleteResult.Succeeded)
                    _logger.LogInformation($"User {body.Email} deleted.");
                return BadRequest();
            }
            _logger.LogInformation($"Confirmation email to user {user.Email} sent.");            
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel body)
        {
            var user = await _userService.GetUserByIdAsync(body.UserId);
            if (user == null)
            {
                _logger.LogWarning($"Invalid email confirmation attempt. User with id {body.UserId} doesn't exist.");
                return BadRequest();
            }

            if (user.EmailConfirmed)
                return Ok();

            var result = await _userService.ConfirmEmailAsync(user, body.Token);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.Email} confirmed email address.");
                return Ok();
            }
            _logger.LogInformation(ControllerExtensions.IdentityErrorBuilder($"Confirmation of email address {body.UserId} failed. Errors: ", result.Errors));
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("sendEmailConfirmTokenAgain")]
        public async Task<IActionResult> SendEmailConfirmTokenAgain([FromBody] SendEmailConfirmTokenAgainModel body)
        {
            body.Email = body.Email.ToLower();
            var user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogWarning($"Invalid send confirmation email attempt. User {body.Email} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Email} neexistuje.");
            }

            if (user.EmailConfirmed)
                return ErrorResponse($"Emailová adresa je už potvrdená.");

            string token = await _userService.GenerateEmailConfirmationTokenAsync(user);
            string callbackUrl = new Uri(_baseUrl, $@"confirmEmail/{user.Id}/{token}").ToString();

            if (!_emailService.SendConfirmationEmail(body.Email, callbackUrl, "RegistrationEmail"))
            {
                _logger.LogError($"Error when sending confirmation email to user {body.Email}.");
                return BadRequest();
            }
            _logger.LogInformation($"Confirmation email to user {user.Email} sent.");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel body)
        {
            body.Email = body.Email.ToLower();
            var user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogInformation($"Invalid login attemp. User {body.Email} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Email} neexistuje.");
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogInformation($"Invalid login attemp. User {body.Email} didn't confirm email address.");
                return StatusCode((int)HttpStatusCode.Forbidden, "Pre prihlásenie prosím potvrď svoju emailovú adresu.");
            }

            var token = await _userService.Authenticate(body.Email, body.Password);
            if (token == null)
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

        [AllowAnonymous]
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel body)
        {
            body.Email = body.Email.ToLower();
            var user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogInformation($"Invalid password reset attemp. User {body.Email} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Email} neexistuje.");
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogInformation($"Invalid password reset attemp. User {body.Email} didn't confirm email address.");
                return ErrorResponse($"Najskôr prosím potvrď svoju emailovú adresu.");
            }

            string token = await _userService.GeneratePasswordResetTokenAsync(user);
            string callbackUrl = new Uri(_baseUrl, $@"set-new-password/{user.Id}/{token}").ToString();
            if (!_emailService.SendConfirmationEmail(body.Email, callbackUrl, "RestorePasswordEmail"))
            {
                _logger.LogError($"Error when sending password reset email to user {body.Email}.");
                return BadRequest();
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("setNewPassword")]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordModel body)
        {
            var user = await _userService.GetUserByIdAsync(body.UserId);
            if (user == null)
            {
                _logger.LogError($"Invalid password reset attemp. User with id {body.UserId} doesn't exist.");
                return BadRequest();
            }
            var result = await _userService.ResetPasswordAsync(user, body.Token, body.Password);
            if (!result.Succeeded)
            {
                _logger.LogInformation(ControllerExtensions.IdentityErrorBuilder($"Error when resetting password for user {user.Email}. Identity errors: ", result.Errors));
                Dictionary<string, string[]> identityErrors = ControllerExtensions.IdentityErrorsToDictionary(result.Errors);
                return ValidationError(identityErrors);
            }
            return Ok();
        }
    }
}
