using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class CourseService : ICourseService
    {
        private readonly IMongoDatabase _database;

        public CourseService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Course entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _database.GetCollection<Course>(nameof(Course)).InsertOneAsync(entityToAdd);
        }

        public async Task<Course> FindByIdAsync(Guid guid)
        {
            var collection = _database.GetCollection<Course>(nameof(Course));
            return await collection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task<Course> FindByNameAsync(string name)
        {
            var collection = _database.GetCollection<Course>(nameof(Course));
            return await collection.Find(x => x.CourseName.Equals(name.ToUpper())).FirstOrDefaultAsync();
        }


    }
}
