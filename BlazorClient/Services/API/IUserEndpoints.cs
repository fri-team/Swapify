using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.UserModels;

namespace BlazorClient.Services.API
{
    public interface IUserEndpoints
    {
        Task<AuthenticatedUserModel> Login(LoginModel loginModel);
    }
}
