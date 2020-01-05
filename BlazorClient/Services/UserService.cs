using BlazorClient.Models.UserModels;
namespace BlazorClient.Services
{
    public class UserService : IUserService
    {
        private AuthenticatedUserModel _user;
        public AuthenticatedUserModel GetUser()
        {
            return _user;
        }

        public void SetUser(AuthenticatedUserModel user)
        {
            _user = user;
        }
    }
}
