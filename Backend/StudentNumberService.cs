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
    public class StudentNumberService : IStudentNumberService
    {
        private readonly ILogger<StudentNumberService> _logger;
        private readonly IMongoDatabase _database;
        private readonly ISchoolScheduleProxy _scheduleProxy;
        private readonly ICourseService _courseService;

        public StudentNumberService(ILogger<StudentNumberService> logger, IMongoDatabase database,
            ISchoolScheduleProxy scheduleProxy, ICourseService courseService)
        {
            _logger = logger;
            _database = database;
            _scheduleProxy = scheduleProxy;
            _courseService = courseService;
        }

        public async Task AddAsync(StudentNumber entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _database.GetCollection<StudentNumber>(nameof(StudentNumber)).InsertOneAsync(entityToAdd);
        }

        public async Task<StudentNumber> FindByIdAsync(Guid guid)
        {
            var collection = _database.GetCollection<StudentNumber>(nameof(StudentNumber));
            return await collection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public virtual async Task<StudentNumber> GetStudentNumberAsync(string studentNumber)
        {
            var collection = _database.GetCollection<StudentNumber>(nameof(StudentNumber));
            var number = await collection.Find(x => x.Number.Equals(studentNumber.ToUpper())).FirstOrDefaultAsync();

            if (number == null)
            {
                var schedule = _scheduleProxy.GetByStudentNumber(studentNumber);
                if (schedule == null)
                {
                    _logger.LogError($"Unable to load schedule for student number {studentNumber}. Schedule proxy returned null");
                    return null;
                }
                Timetable t = await ConverterApiToDomain.ConvertTimetableForStudentNumberAsync(schedule, _courseService);
                Timetable mergedTimetable = ConverterApiToDomain.MergeSameBlocksWithDifferentTeacher(t.AllBlocks);

                number = new StudentNumber();
                number.Timetable = mergedTimetable;
                number.Number = studentNumber;
                number.Courses = mergedTimetable.AllBlocks.Select(x => x.CourseId).ToList();
                await AddAsync(number);
            }
            return number;
        }
    }
}
