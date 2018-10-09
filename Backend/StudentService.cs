using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{
    public class StudentService : IStudentService
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<Student> _studentCollection => _database.GetCollection<Student>(nameof(Student));

        public StudentService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Student entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            AddIdsForBlocks(entityToAdd);
            await _studentCollection.InsertOneAsync(entityToAdd);
        }

        public async Task<Student> FindByIdAsync(Guid guid)
        {
            return await _studentCollection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            AddIdsForBlocks(student);
            await _studentCollection.ReplaceOneAsync(x => x.Id == student.Id, student);
        }

        private void AddIdsForBlocks(Student student)
        {
            if (student.Timetable?.Blocks == null) return;
            foreach (Block blck in student.Timetable.Blocks)
            {
                if (blck.Id == Guid.Empty)
                {
                    blck.Id = Guid.NewGuid();
                }
            }
        }
    }
}
