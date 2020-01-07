using BlazorClient.Models.UserModels;

namespace BlazorClient.Services
{
    public interface IUserService
    {
        AuthenticatedUserModel User { get; set; }
    }
}
