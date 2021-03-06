using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{

    public class BlockChangesService : IBlockChangesService
    {
        private readonly IMongoCollection<BlockChangeRequest> _blockChangesCollection;
        
        public BlockChangesService(IMongoDatabase database)
        {
            _blockChangesCollection = database.GetCollection<BlockChangeRequest>(nameof(BlockChangeRequest));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockChangeRequest"></param>
        /// <returns>bool value if corresponding second BlockChangeRequest was found and the found BlockChangeRequest</returns>
        public async Task<(BlockChangeRequest, BlockChangeRequest)> AddAndFindMatch(BlockChangeRequest blockChangeRequest)
        {
            await AddAsync(blockChangeRequest);
            return await MakeExchangeAndDeleteRequests(blockChangeRequest);
        }

        public async Task<List<BlockChangeRequest>> FindWaitingStudentRequests(Guid studentId)
        {
            return await _blockChangesCollection.Find(
                x => x.StudentId == studentId &&
                     x.Status == ExchangeStatus.WaitingForExchange).ToListAsync();
        }

        public Task<List<BlockChangeRequest>> FindAllStudentRequests(Guid studentId)
        {
            return _blockChangesCollection.Find(x => x.StudentId == studentId).ToListAsync();
        }

        public async Task<bool> CancelExchangeRequest(BlockChangeRequest request)
        {
            BlockChangeRequest a = null;
            if (request.Status == ExchangeStatus.WaitingForExchange)
            {
                a = _blockChangesCollection.FindOneAndDelete(
                    x => x.StudentId == request.StudentId &&
                         x.BlockFrom.CourseId == request.BlockFrom.CourseId &&
                         x.BlockFrom.StartHour == request.BlockFrom.StartHour &&
                         x.BlockFrom.Day == request.BlockFrom.Day &&
                         x.BlockFrom.Duration == request.BlockFrom.Duration &&
                         x.Status == request.Status);
            }
            return (a != null);
        }

        private async Task AddAsync(BlockChangeRequest entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _blockChangesCollection.InsertOneAsync(entityToAdd);
        }

        private async Task<BlockChangeRequest> FindExchange(BlockChangeRequest blockRequest)
        {
            return await _blockChangesCollection.Find(
                x => (x.BlockTo.CourseId == blockRequest.BlockFrom.CourseId &&
                      x.BlockTo.Day == blockRequest.BlockFrom.Day &&
                      x.BlockTo.Duration == blockRequest.BlockFrom.Duration &&
                      x.BlockTo.StartHour == blockRequest.BlockFrom.StartHour &&
                      x.BlockFrom.CourseId == blockRequest.BlockTo.CourseId &&
                      x.BlockFrom.Day == blockRequest.BlockTo.Day &&
                      x.BlockFrom.Duration == blockRequest.BlockTo.Duration &&
                      x.BlockFrom.StartHour == blockRequest.BlockTo.StartHour &&
                      x.StudentId != blockRequest.StudentId &&
                      x.Status != ExchangeStatus.Done)).SortBy(x => x.DateOfCreation).FirstOrDefaultAsync();
        }

        private async Task<(BlockChangeRequest, BlockChangeRequest)> MakeExchangeAndDeleteRequests(BlockChangeRequest request)
        {
            var requestForExchange = await FindExchange(request);
            if (requestForExchange == null)
            {
                return (null, null);
            }
            await SetDoneStatus(request);
            await SetDoneStatus(requestForExchange);
            await RemoveStudentRequests(request);
            await RemoveStudentRequests(requestForExchange);
            return (request, requestForExchange);
        }

        private async Task RemoveStudentRequests(BlockChangeRequest request)
        {
           await _blockChangesCollection.DeleteManyAsync(x => x.StudentId == request.StudentId &&
                                                     x.BlockFrom.CourseId == request.BlockFrom.CourseId &&
                                                     x.BlockFrom.StartHour == request.BlockFrom.StartHour &&
                                                     x.BlockFrom.Day == request.BlockFrom.Day &&
                                                     x.BlockFrom.Duration == request.BlockFrom.Duration &&
                                                     x.Status == ExchangeStatus.WaitingForExchange);
        }

        private async Task SetDoneStatus(BlockChangeRequest request)
        {
            if (request.Status == ExchangeStatus.Done)
            {
                await new Task(() => { throw new ArgumentException("You cannot exchange requests which are already exchanged!"); });
            }
            request.Status = ExchangeStatus.Done;
            await _blockChangesCollection.ReplaceOneAsync(x => x.Id == request.Id, request);
        }
    }
}
