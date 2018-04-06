using System.Threading.Tasks;
using FRITeam.Swapify.Entities;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Add new User, Id will be set by db if not specified
        /// </summary>
        Task AddAsync(User entityToAdd);

    }
}
