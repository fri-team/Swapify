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
        private readonly IMongoDatabase _database;
        private IMongoCollection<BlockChangeRequest> _blockChangesCollection => _database.GetCollection<BlockChangeRequest>(nameof(BlockChangeRequest));

        public BlockChangesService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(BlockChangeRequest entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _blockChangesCollection.InsertOneAsync(entityToAdd);
        }

        public async Task<BlockChangeRequest> FindExchange(BlockChangeRequest blockRequest)
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

        public async Task MakeExchangeAndDeleteRequests(BlockChangeRequest from, BlockChangeRequest to)
        {
            await SetDoneStatus(from);
            await SetDoneStatus(to);
            await RemoveStudentRequests(from.StudentId, from.BlockFrom.CourseId);
            await RemoveStudentRequests(to.StudentId, to.BlockFrom.CourseId);
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
                throw new ArgumentException("You cannot exchange requests which are already exchanged!");
            }
            request.Status = ExchangeStatus.Done;
            await _blockChangesCollection.ReplaceOneAsync(x => x.Id == request.Id, request);
        }
    }
}
