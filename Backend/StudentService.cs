using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{
    public class StudentService : IStudentService
    {
        private readonly IMongoDatabase _database;

        public StudentService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddStudentAsync(Student entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _database.GetCollection<Student>(nameof(Student)).InsertOneAsync(entityToAdd);
        }

        public Student FindStudentById(string guid)
        {
            var collection = _database.GetCollection<Student>(nameof(Student));
            return collection.Find(x=>x.Id.Equals(guid)).FirstOrDefault();
        }
    }
}
