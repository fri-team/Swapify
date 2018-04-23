using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class CourseService : ICourseService
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<Course> _courseCollection => _database.GetCollection<Course>(nameof(Course));

        public CourseService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Course entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _courseCollection.InsertOneAsync(entityToAdd);
        }

        public async Task<Course> FindByIdAsync(Guid guid)
        {
            return await _courseCollection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task<Course> FindByNameAsync(string name)
        {
            return await _courseCollection.Find(x => x.CourseName.Equals(name.ToUpper())).FirstOrDefaultAsync();
        }

        public async Task<Guid> GetOrAddNotExistsCourseId(string courseName, ICourseService courseServ, ISchoolScheduleProxy proxy)
        {
            var course = await courseServ.FindByNameAsync(courseName);
            if (course == null)
            {
                var downloadedTimetable = proxy.GetBySubjectCode(courseName);
                var convertedTimetable = await ConverterApiToDomain.ConvertTimetableForCourseAsync(downloadedTimetable, courseServ, proxy);
                course = new Course() { CourseName = courseName, Timetable = convertedTimetable };
                await courseServ.AddAsync(course);
            }
            return course.Id;

        }
    }
}
