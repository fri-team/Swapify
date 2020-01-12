using BlazorClient.Models.UserModels;
using System;
using System.Threading.Tasks;

namespace BlazorClient.Services.IdentityManagement
{
    public interface IUserService
    {
        /// <summary>
        /// Current authenticated user. Udpated by <see cref="Login"/> and <see cref="GetAuthenticatedUserAsync"/> methods.
        /// </summary>
        AuthenticatedUserModel AuthenticatedUser { get; set; }

        event Action<AuthenticatedUserModel> AuthenticatedUserChanged;

        /// <summary>
        /// Returns and cashes (retrievable from <see cref="AuthenticatedUser"/>) current user.
        /// </summary>
        /// <returns><see cref="AuthenticatedUserModel"/></returns>
        Task<AuthenticatedUserModel> GetAuthenticatedUserAsync();
        
        /// <returns></returns>
        Task<LoginResult> Login(LoginModel loginModel);

        Task Logout();
    }
}
