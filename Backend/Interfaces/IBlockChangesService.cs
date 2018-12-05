using FRITeam.Swapify.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IBlockChangesService
    {
        Task AddAsync(BlockChangeRequest entityToAdd);
        Task<List<BlockChangeRequest>> FindAllStudentRequests(Guid studentId);
        Task<bool> MakeExchangeAndDeleteRequests(BlockChangeRequest request);
        
    }
}
