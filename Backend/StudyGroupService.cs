using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
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

        public async Task<StudyGroup> GetStudyGroupAsync(string studyGroupNumber)
        {
            var collection = _database.GetCollection<StudyGroup>(nameof(StudyGroup));
            var ret = await collection.Find(x => x.GroupName.Equals(studyGroupNumber.ToUpper())).FirstOrDefaultAsync();

            if (ret == null)
            {
                ISchoolScheduleProxy proxy = new SchoolScheduleProxy();
                var schedule = proxy.GetByStudyGroup(studyGroupNumber);
                Timetable t = ConverterApiToDomain.GetStudyGroupTimetable(schedule);
                ret = new StudyGroup();
                ret.Timetable = t;
                //newGroup.GroupName = studyGroupNumber;
                //newGroup.Courses = GetCoursesFromBlocks(t.Blocks);
            }

            return ret;
        }

     
    }
}
