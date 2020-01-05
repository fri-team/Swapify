using BlazorClient.Models.UserModels;

namespace BlazorClient.Services
{
    public interface IUserService
    {
        AuthenticatedUserModel GetUser();
        void SetUser(AuthenticatedUserModel user);
    }
}
