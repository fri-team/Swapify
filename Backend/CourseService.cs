using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;
using FRITeam.Swapify.APIWrapper;
using Microsoft.Extensions.Logging;
using FRITeam.Swapify.Backend.Converter;

namespace FRITeam.Swapify.Backend
{
    public class CourseService : ICourseService
    {
        private readonly ILogger<CourseService> _logger;
        private readonly IMongoDatabase _database;
        private IMongoCollection<Course> _courseCollection => _database.GetCollection<Course>(nameof(Course));
        private readonly ISchoolScheduleProxy _scheduleProxy;

        public CourseService(ILogger<CourseService> logger, IMongoDatabase database, ISchoolScheduleProxy scheduleProxy)
        {
            _logger = logger;
            _database = database;
            _scheduleProxy = scheduleProxy;
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

        public async Task<Course> FindByCodeAsync(string code)
        {
            return await _courseCollection.Find(x => x.CourseCode.Equals(code)).FirstOrDefaultAsync();
        }

        public async Task<Course> FindByNameAsync(string name)
        {
            return await _courseCollection.Find(x => x.CourseName.Equals(name.ToLower())).FirstOrDefaultAsync();
        }

        public List<Course> FindByStartName(string courseStartsWith)
        {
            var filter = "{CourseName: /" + courseStartsWith + "/i }"; //regex for search on every position in course name 
            return _courseCollection.Find(filter).ToList();
        }

        /// <summary>
        /// If course with "courseName" exists function return ID, if course doesnt exist fuction
        /// save this course and return id of saved course.
        /// </summary>
        public async Task<Guid> GetOrAddNotExistsCourseIdByShortcut(string courseShortcut, Block courseBlock)
        {
            var course = await this.FindByCodeAsync(courseShortcut);
            if (course == null)
            {
                var timetable = new Timetable();
                timetable.AddNewBlock(courseBlock);
                course = new Course() {CourseCode = courseShortcut, Timetable = timetable };
                await this.AddAsync(course);
            }
            else
            {
                if (course.Timetable == null)
                {
                    course.Timetable = new Timetable();
                }
                if (!course.Timetable.ContainsBlock(courseBlock))
                {
                    //if course exists but doesnt contain this block
                    //is it neccessary to add it into timetable
                    course.Timetable.AddNewBlock(courseBlock);
                    await this.UpdateAsync(course);
                }
            }
            return course.Id;
        }

        public async Task<Guid> GetOrAddNotExistsCourseIdByName(string courseName, string courseShortcut, Block courseBlock)
        {
            var course = await this.FindByNameAsync(courseName);
            if (course == null)
            {
                var timetable = new Timetable();
                timetable.AddNewBlock(courseBlock);
                course = new Course() { CourseCode = courseShortcut, CourseName = courseName, Timetable = timetable };
                await this.AddAsync(course);
            }
            else
            {
                if (course.Timetable == null)
                {
                    course.Timetable = new Timetable();
                }
                if (!course.Timetable.ContainsBlock(courseBlock))
                {
                    //if course exists but doesnt contain this block
                    //is it neccessary to add it into timetable
                    course.Timetable.AddNewBlock(courseBlock);
                    await this.UpdateAsync(course);
                }
            }
            return course.Id;
        }

        public async Task<Course> FindCourseTimetableFromProxy(Guid guid)
        {
            Course course = await _courseCollection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
            if (course == null)
            {
                _logger.LogError($"Course with id {guid.ToString()} not exist");
                return null;
            }
            var schedule = _scheduleProxy.GetBySubjectCode(course.CourseName);
            if (schedule == null)
            {
                _logger.LogError($"Unable to load schedule for subject {course.CourseCode}. Schedule proxy returned null");
                return null;
            }
            Timetable t = await ConverterApiToDomain.ConvertTimetableForCourseAsync(schedule, this);
            course = new Course();
            course.Timetable = t;
            return course;
        }

        public async Task UpdateAsync(Course course)
        {
            await _courseCollection.ReplaceOneAsync(x => x.Id == course.Id, course);
        }
    }
}
