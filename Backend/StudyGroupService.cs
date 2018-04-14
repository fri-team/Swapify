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
                var converted = ConverterApiToDomain.ConvertTimetable(schedule);
                Timetable t = converted.Item1;
                group = new StudyGroup();
                group.Timetable = t;
                group.GroupName = studyGroupNumber;
                var loadedGroups = await ConverterApiToDomain.GetOrCreateCourseIdFromBlock(converted.Item2, courseServ, proxy);
                group.Courses = loadedGroups.Item1;
            }

            return group;
        }

        private List<Guid> GetCoursesIdsFromBlocks(List<Block> blocks)
        {
            HashSet<Guid> courseSet = new HashSet<Guid>();
            courseSet.UnionWith(blocks.Select(x => x.CourseId));
            return new List<Guid>(courseSet);
        }
    }
}
