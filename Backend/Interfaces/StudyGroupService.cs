using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public class StudyGroupService : IStudyGroupService
    {
        private readonly IMongoDatabase _database;

        public StudyGroupService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(StudyGroup entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _database.GetCollection<StudyGroup>(nameof(StudyGroup)).InsertOneAsync(entityToAdd);
        }

        public async Task<StudyGroup> FindByIdAsync(Guid guid)
        {
            var collection = _database.GetCollection<StudyGroup>(nameof(StudyGroup));
            return await collection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }
    }
}
