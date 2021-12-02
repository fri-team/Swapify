using FRITeam.Swapify.SwapifyBase.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IBlockChangesService
    {
        Task<(BlockChangeRequest, BlockChangeRequest)> AddAndFindMatch(BlockChangeRequest blockChangeRequest);
        Task<List<BlockChangeRequest>> FindAllStudentRequests(Guid studentId);
        Task<List<BlockChangeRequest>> FindWaitingStudentRequests(Guid studentId);
        Task<bool> CancelExchangeRequest(BlockChangeRequest request);
    }
}
