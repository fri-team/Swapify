using BlazorClient.Models.UserModels;
using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public interface IUserService
    {
        AuthenticatedUserModel User { get; set; }
        Task<LoginResult> Login(LoginModel loginModel);
    }
}
