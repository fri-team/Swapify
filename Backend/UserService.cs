using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using FRITeam.Swapify.SwapifyBase.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FRITeam.Swapify.Backend
{
    public class UserService : IUserService
    {
        private readonly EnvironmentSettings _environmentSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly LdapSettings _ldapSettings;
        private readonly ITimetableDataService _studentService;

        public UserService(IOptions<EnvironmentSettings> environmentSettings, UserManager<User> userManager,
            SignInManager<User> signInManager, IOptions<LdapSettings> ldapSettings, ITimetableDataService studentService)
        {
            _environmentSettings = environmentSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _ldapSettings = ldapSettings.Value;
            _studentService = studentService;
        }

        public async Task<JwtSecurityToken> Authenticate(string login, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(login, password, false, false);

            if (result.Succeeded)
                return GenerateJwtToken(login);
            return null;
        }

        public JwtSecurityToken Renew(string jwtToken)
        {
            var secret = Encoding.ASCII.GetBytes(_environmentSettings.JwtSecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(secret)
            };

            var claims = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
            var jwtSecurityToken = validatedToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            var login = claims.Identity.Name;
            return GenerateJwtToken(login);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return Uri.EscapeDataString(token);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Uri.EscapeDataString(token);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            token = Uri.UnescapeDataString(token);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            token = Uri.UnescapeDataString(token);
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> DeleteUserAsyc(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public JwtSecurityToken GenerateJwtToken(string login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_environmentSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, login) }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public async Task<IdentityResult> UpdateUserAsync(User userToUpdate)
        {
            return await _userManager.UpdateAsync(userToUpdate);
        }

        public UserInformations GetUserFromLDAP(string login, string password, ILogger logger)
        {
            login += "@fri.uniza.sk";

            OptionsLdap options = new OptionsLdap
            {
                SecureSocketLayer = bool.Parse(_ldapSettings.SecureSocketLayer),
                BaseDN = _ldapSettings.BaseDN,
                HostName = _ldapSettings.HostName,
                Port = int.Parse(_ldapSettings.Port),
            };
            logger.LogInformation($"Connecting to ldap, SSL: {_ldapSettings.SecureSocketLayer}, HostName: {_ldapSettings.HostName} ");
            AuthenticatorLdap authenticatorLdap = new AuthenticatorLdap(options);
            UserInformations informations = authenticatorLdap.Authenticate(login, password, logger);
            return informations;
        }

        public async Task<bool> AddLdapUser(UserInformations informations)
        {
            string[] names = informations.Name.Split(" ");
            User user = new User(informations.Email, names[0], names[1])
            {
                IsLdapUser = true,
                EmailConfirmed = true
            };
            var addResult = await AddUserAsync(user, GetDefaultLdapPassword());
            return addResult.Succeeded;
        }

        public async void TryAddStudent(User user)
        {
            if (user.TimetableData == null)
            {
                user.TimetableData = new TimetableData
                {
                    UserId = user.Id
                };
                await _studentService.AddAsync(user.TimetableData);
                await UpdateUserAsync(user);
            }
        }

        public string GetDefaultLdapPassword()
        {
            return "Heslo123";
        }
    }
}
