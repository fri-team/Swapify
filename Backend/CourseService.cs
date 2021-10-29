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
using System.Globalization;
using System.Text;

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

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).TrimStart(' ').ToLower();
        }

        public List<Course> FindByStartName(string courseStartsWith, string personalNumber)
        {
            if (courseStartsWith != null && courseStartsWith.Length > 2)
            {
                courseStartsWith = RemoveDiacritics(courseStartsWith);

                List<Course> allCourses = _courseCollection.Find(x => true).ToList();
                List<Course> founded = new List<Course>();
                foreach (var course in allCourses)
                {
                    string courseNameWithoutDia = RemoveDiacritics(course.CourseName);
                    if (courseNameWithoutDia.Length >= courseStartsWith.Length &&
                        courseNameWithoutDia.Substring(0, courseStartsWith.Length).Equals(courseStartsWith))
                        founded.Add(course);
                }

                return founded.OrderByDescending(course => course.CourseCode.First() == personalNumber.First())
                    .ThenBy(course => course.CourseName).ToList();
            } else
            {
                return new List<Course>();
            }
        }

        /// <summary>
        /// If course with exists, function returns ID. If course doesnt exist function
        /// saves this course and returns id of saved course.
        /// </summary>
        public async Task<Course> GetOrAddNotExistsCourse(string courseCode, string courseName)
        {
            var course = await (string.IsNullOrEmpty(courseCode) ? FindByNameAsync(courseName) : FindByCodeAsync(courseCode));
            if (course == null)
            {
                course = new Course() {
                    CourseCode = courseCode,
                    Timetable = new Timetable(Semester.GetSemester()),
                    LastUpdateOfTimetable = null,
                    CourseName = courseName,
                    StudyType = getCourseStudyType(courseCode)
                };                
                if (string.IsNullOrEmpty(courseCode))
                {
                    course.CourseCode = FindCourseCodeFromProxy(course);
                }
                await FindCourseTimetableFromProxy(course);                
                await AddAsync(course);
            }
            else if (IsCourseOutOfDate(course))
            {                                             
                if (string.IsNullOrEmpty(courseCode))                    
                {
                    course.CourseCode = FindCourseCodeFromProxy(course);
                }                
                await FindCourseTimetableFromProxy(course);
                await UpdateAsync(course);                
            }
            return course;
        }
        public string FindCourseCodeFromProxy(Course course)
        {
            var c = _courseProxy.GetByCourseName(course.CourseName).First(c => c.Code.Contains(','));
            if (c != null)
            {
                course.CourseCode = c.Code.Substring(0, c.Code.IndexOf(','));
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
            List<ScheduleTimetable> schedules = new List<ScheduleTimetable>();
            int years = course.StudyType.Contains("inž") ? 2 : 3;
            for (int i = 1; i <= years; i++)
            {
                try
                {
                    ScheduleTimetable schedule = _scheduleProxy.GetBySubjectCode(course.CourseCode, i + "", course.StudyType);
                    if (schedule != null)
                    {
                        schedules.Add(schedule);
                    }
                } catch (Exception ex)
                {
                    _logger.LogWarning($"Error while searching timetable of course {course.CourseName}({course.CourseCode}): {ex.Message}");
                }

                if (schedules == null)
                {
                    _logger.LogError($"Unable to load schedule for subject {course.CourseCode}. Schedule proxy returned null");
                    return null;
                }
            }
            course.Timetable = new Timetable(Semester.GetSemester());

            schedules.ForEach(async (schedule) => {
                Timetable t = await ConverterApiToDomain.ConvertTimetableForCourseAsync(schedule, this);
                if (t != null)
                {
                    foreach (Block b in t.AllBlocks)
                    {
                        if (!course.Timetable.ContainsBlock(b))
                        {
                            course.Timetable.AddNewBlock(b);
                        }
                    }
                }
            });
            course.LastUpdateOfTimetable = DateTime.Now;
            await UpdateAsync(course);            
            return course;            
        }

        public async Task UpdateAsync(Course course)
        {
            await _courseCollection.ReplaceOneAsync(x => x.Id == course.Id, course);
        }

        private bool IsCourseOutOfDate(Course course)
        {
            if (course.Timetable == null || course.LastUpdateOfTimetable == null || course.Timetable.Semester == null) return true;
            TimeSpan difference = (DateTime)course.LastUpdateOfTimetable - DateTime.Now;
            if (Math.Abs(difference.TotalHours) > TIME_PERIOD_IN_HOURS) return true;                                 
            return course.Timetable.IsOutDated();
        }

        private String getCourseStudyType(String subjectCode)
        {
            if (subjectCode[3] == '1')
            {
                return "Denné inžinierske štúdium";
            } else if (subjectCode[3] == '2')
            {
                return "Denné bakalárske štúdium";
            } else
            {
                return "Denné doktorandské štúdium";
            }
        }
    }
}
