using System.Threading.Tasks;
using BlazorClient.Models.UserModels;

namespace BlazorClient.Services.API
{
    public interface IUserEndpoints
    {
        Task<AuthenticatedUserModel> Login(LoginModel loginModel);
        Task<bool> Register(RegisterModel registerModel);
        Task<bool> ResetPassword(ResetPasswordModel resetPasswordModel);
        Task<bool> SetNewPassword(SetNewPasswordModel setNewPasswordModel);
    }
}
