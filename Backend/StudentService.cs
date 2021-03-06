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
            await _studentCollection.InsertOneAsync(entityToAdd);
        }

        public async Task<Student> FindByIdAsync(Guid guid)
        {
            return await _studentCollection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task UpdateStudentAsync(Student studentToUpdate)
        {
            await _studentCollection.ReplaceOneAsync(x => x.Id == studentToUpdate.Id, studentToUpdate);
        }

        public async Task UpdateStudentTimetableAsync(Student studentToUpdate, Timetable studentTimetable)
        {
            studentToUpdate.Timetable = studentTimetable.Clone();
            await UpdateStudentAsync(studentToUpdate);
        }      
    }
}
