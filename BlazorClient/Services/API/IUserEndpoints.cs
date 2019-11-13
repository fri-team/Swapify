using System.Threading.Tasks;
using BlazorClient.Models.UserModels;

namespace BlazorClient.Services.API
{
    public interface IUserEndpoints
    {
        Task<AuthenticatedUserModel> Login(LoginModel loginModel);
    }
}
