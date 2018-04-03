using FRITeam.Swapify.Entities;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IStudentService
    {
        Task AddStudentAsync(Student entityToAdd);

        Task<Student> FindStudentById(string guid);
    }
}
