using CoursesParser;
using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class CourseParserTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;
        public CourseParserTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
        }

        [Fact]
        public async Task ParseFacultiesAsync()
        {
            ////way how to get all courses from elearning to database
            ////*****************************************************************************//

            //IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            //CourseService courseSrv = new CourseService(database);
            //BaseParser parser = new BaseParser();
            ////download from elearning - it can take few minutes!
            //var allCourses = parser.ParseFaculties();

            ////add to database
            //foreach (var course in allCourses)
            //{
            //    var parsed = parser.SplitCodeAndName(course);
            //    var newCourse = new Course { CourseCode = parsed.Item1,
            //                                 CourseName = parsed.Item2,
            //                                 Timetable = new Timetable()
            //                               };
            //    await courseSrv.AddAsync(newCourse);                                            
            //}
            //var ret = await courseSrv.FindByNameAsync("praktikum z programovania 1");
            //ret.Should().NotBeNull();

            ////*****************************************************************************//
        }
    }
}
