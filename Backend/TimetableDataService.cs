using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{
    public class TimetableDataService : ITimetableDataService
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<TimetableData> _timetableDataCollection => _database.GetCollection<TimetableData>(nameof(TimetableData));

        public TimetableDataService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(TimetableData entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _timetableDataCollection.InsertOneAsync(entityToAdd);
        }

        public async Task<TimetableData> FindByIdAsync(Guid guid)
        {
            return await _timetableDataCollection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task UpdateTimetableDataAsync(TimetableData timetableDataToUpdate)
        {
            await _timetableDataCollection.ReplaceOneAsync(x => x.Id == timetableDataToUpdate.Id, timetableDataToUpdate);
        }

        public async Task UpdateTimetableAsync(TimetableData timeTableDataToUpdate, Timetable newTimetable)
        {
            timeTableDataToUpdate.Timetable = newTimetable.Clone();
            await UpdateTimetableDataAsync(timeTableDataToUpdate);
        }      
    }
}
