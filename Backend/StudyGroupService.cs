using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<StudyGroup> GetStudyGroupAsync(string studyGroupNumber, ICourseService courseServ, ISchoolScheduleProxy proxy)
        {
            var collection = _database.GetCollection<StudyGroup>(nameof(StudyGroup));
            var group = await collection.Find(x => x.GroupName.Equals(studyGroupNumber.ToUpper())).FirstOrDefaultAsync();

            if (group == null)
            {
                var schedule = proxy.GetByStudyGroup(studyGroupNumber);
                Timetable t = await ConverterApiToDomain.ConvertTimetableForGroupAsync(schedule, courseServ, proxy);
                group = new StudyGroup();
                group.Timetable = t;
                group.GroupName = studyGroupNumber;
                group.Courses = t.Blocks.Select(x => x.CourseId).ToList();
                await AddAsync(group);

            }

            return group;
        }

    }
}
