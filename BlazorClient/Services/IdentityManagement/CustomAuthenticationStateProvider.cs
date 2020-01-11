using BlazorClient.Models.UserModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorClient.Services.IdentityManagement
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private IUserService _userService;
        public CustomAuthenticationStateProvider(IUserService userService)
        {
            _userService = userService;
            _userService.AuthenticatedUserChanged += OnAuthenticationStateChanged;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authenticatedUser = _userService.AuthenticatedUser;

            if (authenticatedUser == null)
            {
                authenticatedUser = await _userService.GetAuthenticatedUserAsync();                
            }

            return GetAuthenticationState(authenticatedUser);            
        }

        private AuthenticationState GetAuthenticationState(AuthenticatedUserModel authenticatedUser)
        {
            if (authenticatedUser == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, authenticatedUser.Name),
                new Claim(ClaimTypes.Surname, authenticatedUser.Surname),
                new Claim(ClaimTypes.Email, authenticatedUser.Email)
            }, "jwt");

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        private void OnAuthenticationStateChanged(AuthenticatedUserModel authenticatedUser)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(GetAuthenticationState(authenticatedUser)));
        }
    }
}
