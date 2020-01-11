using BlazorClient.Models.UserModels;
using System;
using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public class UserService : IUserService
    {
        private ISwapifyAPI _swapifyAPI;
        private ILocalStorage _localStorage;
        private AuthenticatedUserModel _authenticatedUser;
        
        private const string AuthenticatedUserStorageKey = "AuthenticatedUser";

        public event Action<AuthenticatedUserModel> AuthenticatedUserChanged;

        public UserService(ISwapifyAPI swapifyAPI, ILocalStorage localStorage)
        {
            _swapifyAPI = swapifyAPI;
            _localStorage = localStorage;
        }
        
        public AuthenticatedUserModel AuthenticatedUser
        {
            get => _authenticatedUser;
            set
            {
                _authenticatedUser = value;
                AuthenticatedUserChanged(_authenticatedUser);
            }
        }
        
        public async Task<AuthenticatedUserModel> GetAuthenticatedUserAsync()
        {
            var authenticatedUser = await _localStorage.GetAsync<AuthenticatedUserModel>(AuthenticatedUserStorageKey);
            AuthenticatedUser = authenticatedUser;
            return authenticatedUser;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {            
            var loginResult = await _swapifyAPI.User.Login(loginModel);
            if (loginResult.Successful)
            {
                AuthenticatedUser = loginResult.AuthenticatedUser;
                await _localStorage.SetAsync(AuthenticatedUserStorageKey, loginResult.AuthenticatedUser);                
            }
            return loginResult;
        }
    }
}
