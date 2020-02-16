using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{
    public class StudentService : IStudentService
    {
        private readonly IMongoCollection<Student> _studentCollection;

        public StudentService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _studentCollection = database.GetCollection<Student>(nameof(Student));
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
