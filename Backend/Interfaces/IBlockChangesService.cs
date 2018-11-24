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
        Task<BlockChangeRequest> FindExchange(BlockChangeRequest blockRequest);
        Task<List<BlockChangeRequest>> FindAllStudentRequests(Guid studentId);
        Task MakeExchangeAndDeleteRequests(BlockChangeRequest from, BlockChangeRequest to);
        
    }
}
