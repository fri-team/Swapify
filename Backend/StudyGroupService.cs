using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class StudyGroupService : IStudyGroupService
    {
        private readonly ILogger<StudyGroupService> _logger;
        private readonly IMongoDatabase _database;
        private readonly ISchoolScheduleProxy _scheduleProxy;
        private readonly ICourseService _courseService;

        public StudyGroupService(ILogger<StudyGroupService> logger, IMongoDatabase database,
            ISchoolScheduleProxy scheduleProxy, ICourseService courseService)
        {
            _logger = logger;
            _database = database;
            _scheduleProxy = scheduleProxy;
            _courseService = courseService;

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

        public virtual async Task<StudyGroup> GetStudyGroupAsync(string studyGroupNumber)
        {
            var collection = _database.GetCollection<StudyGroup>(nameof(StudyGroup));
            var group = await collection.Find(x => x.GroupName.Equals(studyGroupNumber.ToUpper())).FirstOrDefaultAsync();

            if (group == null)
            {
                var schedule = _scheduleProxy.GetByStudyGroup(studyGroupNumber);
                if (schedule == null)
                {
                    _logger.LogError($"Unable to load schedule for study group {studyGroupNumber}. Schedule proxy returned null");
                    return null;
                }
                Timetable t = await ConverterApiToDomain.ConvertTimetableForGroupAsync(schedule, _courseService);
                group = new StudyGroup();
                group.Timetable = t;
                group.GroupName = studyGroupNumber;
                group.Courses = t.AllBlocks.Select(x => x.CourseId).ToList();
                await AddAsync(group);

            }

            return group;
        }
    }
}
