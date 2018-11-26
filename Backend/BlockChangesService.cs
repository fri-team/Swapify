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

        public async Task AddAsync(BlockChangeRequest entityToAdd)
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
                      x.Status != ExchangeStatus.Done)).FirstOrDefaultAsync();
        }

        public Task<List<BlockChangeRequest>> FindAllStudentRequests(Guid studentId)
        {
            return _blockChangesCollection.Find(x => x.StudentId == studentId).ToListAsync();
        }

        public async Task<bool> MakeExchangeAndDeleteRequests(BlockChangeRequest request)
        {
            var requestForExchange = await FindExchange(request);
            if (request == null)
            {
                return false;
            }
            await SetDoneStatus(request);
            await SetDoneStatus(requestForExchange);
            await RemoveStudentRequests(request.StudentId, request.BlockFrom.CourseId);
            await RemoveStudentRequests(requestForExchange.StudentId, requestForExchange.BlockFrom.CourseId);
            return true;
        }

        private async Task RemoveStudentRequests(Guid studentId, Guid courseId)
        {
            await _blockChangesCollection.DeleteManyAsync(x => x.StudentId == studentId &&
                                                     x.BlockFrom.CourseId == courseId);
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
