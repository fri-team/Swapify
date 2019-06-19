using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{
    public class PersonalNumberService : IPersonalNumberService
    {
        private readonly ILogger<PersonalNumberService> _logger;
        private readonly IMongoDatabase _database;
        private readonly ISchoolScheduleProxy _scheduleProxy;
        private readonly ICourseService _courseService;

        public PersonalNumberService(ILogger<PersonalNumberService> logger, IMongoDatabase database,
            ISchoolScheduleProxy scheduleProxy, ICourseService courseService)
        {
            _logger = logger;
            _database = database;
            _scheduleProxy = scheduleProxy;
            _courseService = courseService;
        }

        public async Task AddAsync(PersonalNumber entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _database.GetCollection<PersonalNumber>(nameof(PersonalNumber)).InsertOneAsync(entityToAdd);
        }

        public async Task<PersonalNumber> FindByIdAsync(Guid guid)
        {
            var collection = _database.GetCollection<PersonalNumber>(nameof(PersonalNumber));
            return await collection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public virtual async Task<PersonalNumber> GetPersonalNumberAsync(string personalNumber)
        {
            var collection = _database.GetCollection<PersonalNumber>(nameof(PersonalNumber));
            var number = await collection.Find(x => x.Number.Equals(personalNumber.ToUpper())).FirstOrDefaultAsync();

            if (number == null)
            {
                var schedule = _scheduleProxy.GetByPersonalNumber(personalNumber);
                if (schedule == null)
                {
                    _logger.LogError($"Unable to load schedule for student number {personalNumber}. Schedule proxy returned null");
                    return null;
                }
                Timetable t = await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(schedule, _courseService);
                Timetable mergedTimetable = ConverterApiToDomain.MergeSameBlocksWithDifferentTeacher(t.AllBlocks);

                number = new PersonalNumber();
                number.Timetable = mergedTimetable;
                number.Number = personalNumber;
                number.Courses = mergedTimetable.AllBlocks.Select(x => x.CourseId).ToList();
                await AddAsync(number);
            }
            return number;
        }
    }
}
