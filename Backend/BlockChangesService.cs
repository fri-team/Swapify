using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;
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
            var changeRequest = await FindOrAddAsync(blockChangeRequest);
            return await MakeExchangeAndDeleteRequests(changeRequest);
        }

        public async Task<List<BlockChangeRequest>> FindWaitingStudentRequests(Guid timetableId)
        {
            return await _blockChangesCollection.Find(
                x => x.TimetableId == timetableId &&
                     x.Status == ExchangeStatus.WaitingForExchange).ToListAsync();
        }

        public Task<List<BlockChangeRequest>> FindAllStudentRequests(Guid timetableId)
        {
            return _blockChangesCollection.Find(x => x.TimetableId == timetableId).ToListAsync();
        }

        public async Task<bool> CancelExchangeRequest(BlockChangeRequest request)
        {
            BlockChangeRequest a = null;
            if (request.Status == ExchangeStatus.WaitingForExchange)
            {
                a = await _blockChangesCollection.FindOneAndDeleteAsync(
                    x => x.TimetableId == request.TimetableId &&
                         x.BlockFrom.CourseId == request.BlockFrom.CourseId &&
                         x.BlockFrom.StartHour == request.BlockFrom.StartHour &&
                         x.BlockFrom.Day == request.BlockFrom.Day &&
                         x.BlockFrom.Duration == request.BlockFrom.Duration &&
                         x.Status == request.Status);
            }
            return (a != null);
        }

        private async Task<BlockChangeRequest> FindOrAddAsync(BlockChangeRequest entityToFindOrAdd)
        {
            var request = await _blockChangesCollection.Find(x =>
            (x.TimetableId == entityToFindOrAdd.TimetableId &&
                      x.BlockTo.CourseId == entityToFindOrAdd.BlockTo.CourseId &&
                      x.BlockTo.Day == entityToFindOrAdd.BlockTo.Day &&
                      x.BlockTo.Duration == entityToFindOrAdd.BlockTo.Duration &&
                      x.BlockTo.StartHour == entityToFindOrAdd.BlockTo.StartHour &&
                      x.BlockTo.Room == entityToFindOrAdd.BlockTo.Room &&
                      x.BlockFrom.CourseId == entityToFindOrAdd.BlockFrom.CourseId &&
                      x.BlockFrom.Room == entityToFindOrAdd.BlockFrom.Room &&
                      x.BlockFrom.Day == entityToFindOrAdd.BlockFrom.Day &&
                      x.BlockFrom.Duration == entityToFindOrAdd.BlockFrom.Duration &&
                      x.BlockFrom.StartHour == entityToFindOrAdd.BlockFrom.StartHour)
                ).FirstOrDefaultAsync();
            if (request == null)
            {
                entityToFindOrAdd.Id = Guid.NewGuid();
                await _blockChangesCollection.InsertOneAsync(entityToFindOrAdd);
                // if there is no same request, returning inserted request
                return entityToFindOrAdd;
            }
            // if there is same request, returning request that is already in database 
            else
                return request;
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
                      x.TimetableId != blockRequest.TimetableId &&
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
           await _blockChangesCollection.DeleteManyAsync(x => x.TimetableId == request.TimetableId &&
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
