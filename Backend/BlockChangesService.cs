using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{

    public class BlockChangesService : IBlockChangesService
    {
        private readonly IMongoCollection<BlockChangeRequest> _blockChangesCollection;
        private static ConcurrentDictionary<BlockChangeRequest, object> _lockObjects = new ConcurrentDictionary<BlockChangeRequest, object>();

        public BlockChangesService(IMongoDatabase database)
        {
            _blockChangesCollection = database.GetCollection<BlockChangeRequest>(nameof(BlockChangeRequest));
        }

        public async Task<bool> AddAndFindMatch(BlockChangeRequest entityToAdd)
        {
            await AddAsync(entityToAdd);
            return await MakeExchangeAndDeleteRequests(entityToAdd);
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
            DeleteResult a = null;
            var lockObject = _lockObjects.GetOrAdd(request, new object());
            lock (lockObject)
            {
                try
                {
                    if (request.Status == ExchangeStatus.WaitingForExchange)
                    {
                        a = _blockChangesCollection.DeleteOneAsync(
                            x => x.StudentId == request.StudentId &&
                                 x.BlockFrom.CourseId == request.BlockFrom.CourseId &&
                                 x.BlockFrom.StartHour == request.BlockFrom.StartHour &&
                                 x.BlockFrom.Day == request.BlockFrom.Day &&
                                 x.BlockFrom.Duration == request.BlockFrom.Duration &&
                                 x.Status == request.Status).Result;
                    }
                }
                finally
                {
                    _lockObjects.TryRemove(request, out _);
                }
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

        private async Task<bool> MakeExchangeAndDeleteRequests(BlockChangeRequest request)
        {
            var requestForExchange = await FindExchange(request);
            if (requestForExchange == null)
            {
                return false;
            }
            await SetDoneStatus(request);
            await SetDoneStatus(requestForExchange);
            await RemoveStudentRequests(request);
            await RemoveStudentRequests(requestForExchange);
            return true;
        }

        private async Task RemoveStudentRequests(BlockChangeRequest request)
        {
            await _blockChangesCollection.DeleteManyAsync(x => x.StudentId == request.StudentId &&
                                                     x.BlockFrom.CourseId == request.BlockFrom.CourseId &&
                                                     x.BlockFrom.StartHour == request.BlockFrom.StartHour &&
                                                     x.BlockFrom.Day == request.BlockFrom.Day &&
                                                     x.BlockFrom.Duration == request.BlockFrom.Duration);
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
