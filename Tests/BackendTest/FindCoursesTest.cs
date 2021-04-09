using FluentAssertions;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class FindCoursesTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;
        private readonly IMongoDatabase _database;
        private readonly Mock<ILogger<CourseService>> _loggerMockCourse;

        public FindCoursesTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
            _loggerMockCourse = new Mock<ILogger<CourseService>>();
            _database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");        
        }

        [Fact]
        public async Task FindCoursetWithoutWritingAccents()
        {
            var schoolScheduleProxy = new SchoolScheduleProxy();
            var schoolCourseProxy = new SchoolCourseProxy();
            CourseService courseService = new CourseService(_loggerMockCourse.Object, _database, schoolScheduleProxy, schoolCourseProxy);

            Course course = new Course
            {
                CourseCode = "5BH118",
                CourseName = "číslicové počítače",
            };
            await courseService.AddAsync(course);

            List<Course> courses = courseService.FindByStartName("cisl", "557317");
            courses.FindAll(x => x.CourseName.StartsWith("čísl")).Should().NotBeEmpty();
        }

        [Fact]
        public async Task FindCourseWithWritingAccents()
        {
            var schoolScheduleProxy = new SchoolScheduleProxy();
            var schoolCourseProxy = new SchoolCourseProxy();
            CourseService courseService = new CourseService(_loggerMockCourse.Object, _database, schoolScheduleProxy, schoolCourseProxy);

            Course course = new Course
            {
                CourseCode = "5BH118",
                CourseName = "číslicové počítače",
            };
            await courseService.AddAsync(course);

            List<Course> courses = courseService.FindByStartName("čí", "557317");
            courses.FindAll(x => x.CourseName.StartsWith("čísl")).Should().NotBeNull();
        }

        [Fact]
        public async Task FindCoursesAndReturnItInSortedWay()
        {
            var schoolScheduleProxy = new SchoolScheduleProxy();
            var schoolCourseProxy = new SchoolCourseProxy();
            CourseService courseService = new CourseService(_loggerMockCourse.Object, _database, schoolScheduleProxy, schoolCourseProxy);

            Course course = new Course
            {
                CourseCode = "211025",
                CourseName = "materiály II",
            };

            Course course1 = new Course
            {
                CourseCode = "9BD002",
                CourseName = "matematika 1",
            };

            Course course2 = new Course
            {
                CourseCode = "5BF117",
                CourseName = "matematická analýza",
            };

            Course course3 = new Course
            {
                CourseCode = "41B101",
                CourseName = "matematický seminár",
            };

            await courseService.AddAsync(course);
            await courseService.AddAsync(course1);
            await courseService.AddAsync(course2);
            await courseService.AddAsync(course3);

            List<Course> courses = courseService.FindByStartName("mat", "957317");
            courses[0].CourseName.Should().Be("matematika 1");

            courses = courseService.FindByStartName("mat", "457317");
            courses[0].CourseName.Should().Be("matematický seminár");

            courses = courseService.FindByStartName("mat", "557317");
            courses[0].CourseName.Should().Be("matematická analýza");

            courses = courseService.FindByStartName("mat", "257317");
            courses[0].CourseName.Should().Be("materiály II");
        }
    }
}
