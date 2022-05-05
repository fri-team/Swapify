using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Novell.Directory.Ldap;
using WebAPI.Extensions;
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
            if (_emailService.GetCaptchaNotPassed(body.Captcha).Result)
            {
                return ErrorResponse($"Prvok Re-Captcha je zlý skúste znova.");
            }

            body.Email = body.Email.ToLower();
            User user = new User(body.Email, body.Name, body.Surname);
            var addResult = await _userService.AddUserAsync(user, body.Password);

            if (!addResult.Succeeded)
            {
                var existingUser = await _userService.GetUserByEmailAsync(body.Email);
                // user hasn't confirmed email
                if (existingUser != null && !existingUser.EmailConfirmed)
                {
                    Console.WriteLine("existingUser: " + existingUser);
                    string newToken = await _userService.GenerateEmailConfirmationTokenAsync(existingUser);
                    string newCallbackUrl = new Uri(_baseUrl, $@"confirmEmail/{existingUser.Id}/{newToken}").ToString();

                    if (!_emailService.SendConfirmationEmail(body.Email, newCallbackUrl, "RegistrationEmail"))
                    {
                        _logger.LogError($"Error when sending confirmation email to user {body.Email}. Errors: {addResult.Errors} URI: {newCallbackUrl}");
                        return BadRequest();
                    }
                    _logger.LogInformation($"Confirmation email to user {user.Email} sent.");
                    return Ok();
                }
                else
                {
                    _logger.LogInformation(ControllerExtensions.IdentityErrorBuilder($"Error when creating user {body.Email}. Identity errors: ", addResult.Errors));
                    Dictionary<string, string[]> identityErrors = ControllerExtensions.IdentityErrorsToDictionary(addResult.Errors);
                    return ValidationError(identityErrors);
                }
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
        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserModel body)
        {
            body.Email = body.Email.ToLower();
            var user = await _userService.GetUserByEmailAsync(body.Email);

            if (user == null)
            {
                _logger.LogInformation($"Invalid user delete attemp. User {body.Email} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Email} neexistuje.");
            }

            if (user.IsLdapUser)
            {
                UserInformations ldapInfo = _userService.GetUserFromLDAP(body.Email.Split('@')[0], body.Password, _logger);
                if (ldapInfo == null)
                {
                    _logger.LogWarning($"Invalid login attemp. User {body.Email} entered wrong password.");
                    return ErrorResponse("Zadané heslo nie je správne.");
                }
            }
            else
            {
                var token = await _userService.Authenticate(body.Email, body.Password);
                if (token == null)
                {
                    _logger.LogWarning($"Invalid login attemp. User {body.Email} entered wrong password.");
                    return ErrorResponse("Zadané heslo nie je správne.");
                }
            }

            var deleteResult = await _userService.DeleteUserAsyc(user);
            if (deleteResult.Succeeded)
                _logger.LogInformation($"User {body.Email} deleted.");

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
                return Ok();
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
        [HttpPost("loginLdap")]
        public async Task<IActionResult> LoginLdap([FromBody] LoginModel body)
        {
            if (await _emailService.GetCaptchaNotPassed(body.Captcha))
            {
                return ErrorResponse($"Prvok Re-Captcha je zlý skúste znova.");
            }

            if (body.Email.Contains('@'))
            {
                return ErrorResponse($"Meno nie je správne, použite študentské meno, nie email.");
            }
            _logger.LogInformation($"Request init from ldap.");
            UserInformations ldapInformations = null;
            try
            {
                ldapInformations = _userService.GetUserFromLDAP(body.Email, body.Password, _logger);
            } catch (LdapException e)
            {
                var old_mail_format = ldapInformations.Email.Split('@')[0] + "@fri.uniza.sk";
                User old_user = await _userService.GetUserByEmailAsync(old_mail_format);
                if (old_user != null) {
                    _logger.LogError($"User {body.Email} try to use FRI LDAP login.");
                    var result = await _userService.ChangeUserEmail(old_user, body.Email);
                    if (!result.Succeeded)
                    {
                        _logger.LogError($"Cannot change fri email to uniza for user {body.Email}.");
                    }.
                    else {
                        _logger.LogError($"We succesfuly change email for user {body.Email}.");
                    }
                    return ErrorResponse($"Použite heslo rovnake ako do vzdelávania.");
                }
                else {
                    _logger.LogError($"Exception while logging into ldap: {e}");
                    return ErrorResponse(e.ResultCode == 49 ? $"Meno alebo heslo nie je správne, skúste znova prosím." : $"Prepáčte, niečo na serveri nie je v poriadku, skúste neskôr prosím."); //49 = InvalidCredentials
                }
            }                    
            _logger.LogInformation($"Response received from ldap.");
            body.Password = _userService.GetDefaultLdapPassword();

            if (ldapInformations == null)
            {
                _logger.LogInformation($"Invalid ldap login attemp. User {body.Email} doesn't exist.");
                return ErrorResponse($"Meno alebo heslo nie je správne, skúste znova prosím.");
            }
            ldapInformations.Email = ldapInformations.Email.ToLower();
            User user = await _userService.GetUserByEmailAsync(ldapInformations.Email);            
            if (user == null)
            {
                if (!_userService.AddLdapUser(ldapInformations).Result)
                {
                    _logger.LogInformation($"Invalid ldap login attemp. User with email {body.Email} already exists.");
                    return ErrorResponse($"Váš študentský email s koncovkou " + ldapInformations.Email.Split('@')[1] + " je už zaregistrovaný.");
                }
                user = await _userService.GetUserByEmailAsync(ldapInformations.Email);
                _userService.TryAddStudent(user);
                AuthenticatedUserModel auth = new AuthenticatedUserModel(user, _userService.GenerateJwtToken(ldapInformations.Email))
                {
                    FirstTimePN = ldapInformations.PersonalNumber
                };
                return Ok(auth);
            }
            else
            {
                _userService.TryAddStudent(user);
                AuthenticatedUserModel auth = new AuthenticatedUserModel(user, _userService.GenerateJwtToken(ldapInformations.Email));
                return Ok(auth);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel body)
        {
            try
            {                
                if (await _emailService.GetCaptchaNotPassed(body.Captcha))
                {
                    return ErrorResponse($"Prvok Re-Captcha je zlý skúste znova.");
                }
                if (!body.Email.Contains('@'))
                {
                    return ErrorResponse($"Zlý email.");
                }                
                body.Email = body.Email.ToLower();
                User user = await _userService.GetUserByEmailAsync(body.Email);
                if (user == null)
                {
                    _logger.LogInformation($"Invalid login attemp. User {body.Email} doesn't exist.");
                    return ErrorResponse($"E-mailová adresa a heslo nie sú správne.");
                }
                if (user.IsLdapUser)
                {
                    _logger.LogInformation($"User {body.Email} is ldap user no classic.");
                    return ErrorResponse($"Musite sa prihlásiť cez Ldap prihlásenie, pretože email je študentský.");
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
                    return ErrorResponse($"E-mailová adresa a heslo nie sú správne.");
                }

                if (user.Email != "tester@testovaci.com")
                    _userService.TryAddStudent(user);
                token = await _userService.Authenticate(body.Email, body.Password);
                AuthenticatedUserModel authUser = new AuthenticatedUserModel(user, token);
                return Ok(authUser);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Error HERE: " + ex);
            }
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
                return Ok();
            }

            if (user.IsLdapUser)
            {
                _logger.LogInformation($"Password reset attemp for Ldap user {body.Email}.");
                return ErrorResponse($"Heslo pre Ldap používateľa nemožno zmeniť.");
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

        [AllowAnonymous]
        [HttpPost("sendFeedback")]
        public async Task<IActionResult> SendFeedback([FromBody] FeedbackModel body)
        {
            body.Email = body.Email.ToLower();

            var user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogInformation($"Invalid sending feedback attemp. User {body.Email} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Email} neexistuje.");
            }

            if (!_emailService.SendFeedbackEmail(body.Email, body.Name, body.Subject, body.Body))
            {
                _logger.LogError($"Error when sending feedback email from user {body.Email}.");
                return BadRequest();
            }
            _logger.LogInformation($"Feedback email from user {body.Email} sent.");

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("setDarkMode")]
        public async Task<IActionResult> ChangeDarkModeForUser([FromBody] DarkModeModel body)
        {
            body.Email = body.Email.ToLower();

            var user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogInformation($"Invalid dark mode change. User {body.Email} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Email} neexistuje.");
            }

            user.DarkMode = body.DarkMode.Equals("true");
            await _userService.UpdateUserAsync(user);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("getDarkMode")]
        public async Task<IActionResult> GetDarkModeForUser([FromBody] DarkModeModel body)
        {
            body.Email = body.Email.ToLower();

            var user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogInformation($"Invalid dark mode change. User {body.Email} doesn't exist.");
                return ErrorResponse($"Používateľ {body.Email} neexistuje.");
            }
            return Ok(user.DarkMode);
        }
    }
}
