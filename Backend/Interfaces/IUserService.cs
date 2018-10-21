using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IUserService
    {
        JwtSecurityToken Authenticate(string login, string password);
        Task AddAsync(User entityToAdd);
    }
}
