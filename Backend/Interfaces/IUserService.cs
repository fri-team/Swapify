using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Identity;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IUserService
    {
        Task<JwtSecurityToken> Authenticate(string login, string password);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<User> GetUserAsync(string email);
        Task<IdentityResult> DeleteUserAsyc(User user);
        JwtSecurityToken Renew(string jwtToken);
    }
}
