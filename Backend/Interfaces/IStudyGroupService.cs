using FRITeam.Swapify.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IStudyGroupService
    {
        Task AddAsync(StudyGroup entityToAdd);
        Task<StudyGroup> FindByIdAsync(Guid guid);
        Task<StudyGroup> GetStudyGroupAsync(string studyGroupNumber);
        Task<List<StudyGroup>> GetStudyGroupList(string studyGroupNumber);

    }
}
