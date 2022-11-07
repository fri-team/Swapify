using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{
    public class BaseUserService : IBaseUserService
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<BaseUser> _usersCollection => _database.GetCollection<BaseUser>("Student");

        public BaseUserService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(BaseUser entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _usersCollection.InsertOneAsync(entityToAdd);
        }

        public async Task<BaseUser> FindByIdAsync(Guid guid)
        {
            return await _usersCollection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task UpdateStudentAsync(BaseUser userToUpdate)
        {
            await _usersCollection.ReplaceOneAsync(x => x.Id == userToUpdate.Id, userToUpdate);
        }

        public async Task UpdateStudentTimetableAsync(BaseUser userToUpdate, Timetable userTimetable)
        {
            userToUpdate.Timetable = userTimetable.Clone();
            await UpdateStudentAsync(userToUpdate);
        }      
    }
}
