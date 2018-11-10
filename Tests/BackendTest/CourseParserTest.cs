using CoursesParser;
using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;
using System.IO;
using System.Linq;
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
            //int id = 1;
            //var allCourses = parser.ParseFaculties().Select(x =>
            //new { Id = id++,
            //      CourseCode = parser.SplitCodeAndName(x).Item1,
            //      CourseName = parser.SplitCodeAndName(x).Item2
            //});
                        
            
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(allCourses);
            //File.WriteAllText("courses.json", json);

           
            ////*****************************************************************************//
        }
    }
}
