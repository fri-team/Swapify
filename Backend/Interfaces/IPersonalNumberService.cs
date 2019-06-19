using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IPersonalNumberService
    {
        Task AddAsync(PersonalNumber entityToAdd);
        Task<PersonalNumber> FindByIdAsync(Guid guid);
        Task<PersonalNumber> GetPersonalNumberAsync(string studentNumber);
    }
}
