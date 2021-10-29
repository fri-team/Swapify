using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IUserService
    {
        Task<JwtSecurityToken> Authenticate(string login, string password);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(string userId);
        Task<IdentityResult> DeleteUserAsyc(User user);
        Task<IdentityResult> UpdateUserAsync(User userToUpdate);
        JwtSecurityToken Renew(string jwtToken);
        UserInformations GetUserFromLDAP(string login, string password, ILogger logger);
        Task<bool> AddLdapUser(UserInformations informations);
        string GetDefaultLdapPassword();
        JwtSecurityToken GenerateJwtToken(string login);
        public void TryAddStudent(User user);
    }
}
