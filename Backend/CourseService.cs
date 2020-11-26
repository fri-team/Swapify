using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;
using FRITeam.Swapify.APIWrapper;
using Microsoft.Extensions.Logging;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.APIWrapper.Objects;

namespace FRITeam.Swapify.Backend
{
    public class CourseService : ICourseService
    {
        private readonly ILogger<CourseService> _logger;
        private readonly IMongoDatabase _database;
        private IMongoCollection<Course> _courseCollection => _database.GetCollection<Course>(nameof(Course));
        private readonly ISchoolScheduleProxy _scheduleProxy;
        private readonly ISchoolCourseProxy _courseProxy;
        private const int TIME_PERIOD_IN_HOURS = 12;

        public CourseService(ILogger<CourseService> logger, IMongoDatabase database, ISchoolScheduleProxy scheduleProxy, ISchoolCourseProxy courseProxy)
        {
            _logger = logger;
            _database = database;
            _scheduleProxy = scheduleProxy;
            _courseProxy = courseProxy;
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
            return _courseCollection.Find(x => x.CourseName.ToLower().Contains(courseStartsWith.ToLower())).ToList();
        }

        /// <summary>
        /// If course with exists, function returns ID. If course doesnt exist function
        /// saves this course and returns id of saved course.
        /// </summary>
        public async Task<Course> GetOrAddNotExistsCourse(string courseCode, string courseName)
        {
            var course = await (string.IsNullOrEmpty(courseCode) ? this.FindByNameAsync(courseName) : this.FindByCodeAsync(courseCode));
            if (course == null)
            {
                var timetable = new Timetable();
                course = new Course() { CourseCode = courseCode, Timetable = timetable, LastUpdateOfTimetable = null, CourseName = courseName };
                if (string.IsNullOrEmpty(courseCode))
                {
                    course.CourseCode = FindCourseShortCutFromProxy(course);
                }
                await FindCourseTimetableFromProxy(course);                
                await this.AddAsync(course);
            }
            else
            {
                if (course.Timetable == null || course.LastUpdateOfTimetable == null)
                {
                    course.Timetable = new Timetable();
                    if (string.IsNullOrEmpty(courseCode))                    
                    {
                        course.CourseCode = FindCourseShortCutFromProxy(course);
                    }
                    await this.FindCourseTimetableFromProxy(course);
                    await this.UpdateAsync(course);
                }
            }
            return course;
        }
        public string FindCourseShortCutFromProxy(Course course)
        {            
            foreach (var _course in _courseProxy.GetByCourseName(course.CourseName))
            {
                if (_course.ShortCut.Contains(','))
                {
                    course.CourseCode = _course.ShortCut.Substring(0, _course.ShortCut.IndexOf(','));
                    break;
                }                
            }
            return course.CourseCode;
        }

        public async Task<Course> FindCourseTimetableFromProxy(Guid guid)
        {
            Course course = await _courseCollection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
            if (course == null)
            {
                _logger.LogError($"Course with id {guid.ToString()} not exist");
                return null;
            }
            return await FindCourseTimetableFromProxy(course);            
        }

        public async Task<Course> FindCourseTimetableFromProxy(Course course)
        {                       
            var isOutDated = false;
            if (course.LastUpdateOfTimetable != null)
            {
                TimeSpan difference = (DateTime)course.LastUpdateOfTimetable - DateTime.Now;
                if (Math.Abs(difference.TotalHours) > TIME_PERIOD_IN_HOURS) isOutDated = true;
            }
            if (course.LastUpdateOfTimetable == null || isOutDated)
            {
                IEnumerable<ScheduleHourContent> schedule = null;
                try
                {
                    schedule = _scheduleProxy.GetBySubjectCode(course.CourseCode);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Error while searching timetable of course {course.CourseName}({course.CourseCode}): {ex.Message}");
                }                
                if (schedule == null)
                {
                    _logger.LogError($"Unable to load schedule for subject {course.CourseCode}. Schedule proxy returned null");
                    return null;
                }
                Timetable timetable = await ConverterApiToDomain.ConvertTimetableForCourseAsync(schedule, this);
                course.Timetable = timetable;
                course.LastUpdateOfTimetable = System.DateTime.Now;
                await UpdateAsync(course);
            }
            return course;            
        }

        public async Task UpdateAsync(Course course)
        {
            await _courseCollection.ReplaceOneAsync(x => x.Id == course.Id, course);
        }
    }
}
