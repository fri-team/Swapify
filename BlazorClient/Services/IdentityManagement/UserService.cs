using BlazorClient.Models.UserModels;
using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public class UserService : IUserService
    {
        private ISwapifyAPI _swapifyAPI;
        private ILocalStorage _localStorage;

        private const string AuthenticatedUserStorageKey = "AuthenticatedUser";

        public UserService(ISwapifyAPI swapifyAPI, ILocalStorage localStorage)
        {
            _swapifyAPI = swapifyAPI;
            _localStorage = localStorage;
        }

        public AuthenticatedUserModel User { get; set; }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var authenticatedUserModel = await _localStorage.GetAsync<AuthenticatedUserModel>(AuthenticatedUserStorageKey);
            if (authenticatedUserModel != null)
            {
                User = authenticatedUserModel;
                return new LoginResult()
                {
                    Successful = true,
                    AuthenticatedUser = authenticatedUserModel
                };
            }

            var loginResult = await _swapifyAPI.User.Login(loginModel);
            if (loginResult.Successful)
            {
                User = loginResult.AuthenticatedUser;
                await _localStorage.SetAsync(AuthenticatedUserStorageKey, loginResult.AuthenticatedUser);                
            }
            return loginResult;
        }
    }
}
