using BlazorClient.Models.UserModels;
namespace BlazorClient.Services
{
    public class UserService : IUserService
    {
        public AuthenticatedUserModel User { get; set; }        
    }
}
