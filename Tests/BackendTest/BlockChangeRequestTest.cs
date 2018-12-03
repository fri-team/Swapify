using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using MongoDB.Driver;
using System.Threading.Tasks;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class BlockChangeRequestText : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;

        public BlockChangeRequestText(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
        }

        [Fact]
        public async Task ExchangeRequest()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            StudentService studentSrv = new StudentService(database);
            BlockChangesService blockChangeService = new BlockChangesService(database);
            CourseService courseService = new CourseService(database);

            Course course = new Course();
            course.CourseCode = "11111";
            course.CourseName = "Programovanie";
            await courseService.AddAsync(course);

            Course course2 = new Course();
            course2.CourseCode = "11111";
            course2.CourseName = "Programovanie";
            await courseService.AddAsync(course2);

            Block block1 = new Block()
            {
                BlockType = BlockType.Laboratory,
                Day = Day.Monday,
                Duration = 2,
                StartHour = 7,
                CourseId = course.Id
            };
            Block block2 = new Block()
            {
                BlockType = BlockType.Laboratory,
                Day = Day.Wednesday,
                Duration = 2,
                StartHour = 10,
                CourseId = course.Id
            };

            Block block3 = new Block()
            {
                BlockType = BlockType.Laboratory,
                Day = Day.Tuesday,
                Duration = 2,
                StartHour = 15,
                CourseId = course.Id
            };

            Block block4 = new Block()
            {
                BlockType = BlockType.Laboratory,
                Day = Day.Friday,
                Duration = 2,
                StartHour = 18,
                CourseId = course2.Id
            };
            Student student1 = new Student();
            Student student2 = new Student();
            Student student3 = new Student();
            await studentSrv.AddAsync(student1);
            await studentSrv.AddAsync(student2);
            await studentSrv.AddAsync(student3);

            BlockChangeRequest blockToChange1 = new BlockChangeRequest();
            blockToChange1.BlockFrom = block1.Clone();
            blockToChange1.BlockTo = block2.Clone();
            blockToChange1.StudentId = student1.Id;
            await blockChangeService.AddAsync(blockToChange1);

            BlockChangeRequest blockToChange2 = new BlockChangeRequest();
            blockToChange2.BlockFrom = block1.Clone();
            blockToChange2.BlockTo = block3.Clone();
            blockToChange2.StudentId = student1.Id;
            await blockChangeService.AddAsync(blockToChange2);

            BlockChangeRequest blockToChange3 = new BlockChangeRequest();
            blockToChange3.BlockFrom = block1.Clone();
            blockToChange3.BlockTo = block2.Clone();
            blockToChange3.StudentId = student2.Id;
            await blockChangeService.AddAsync(blockToChange3);

            BlockChangeRequest blockToChange4 = new BlockChangeRequest();
            blockToChange4.BlockFrom = block1.Clone();
            blockToChange4.BlockTo = block3.Clone();
            blockToChange4.StudentId = student2.Id;
            await blockChangeService.AddAsync(blockToChange4);

            BlockChangeRequest blockToChange5 = new BlockChangeRequest();
            blockToChange5.BlockFrom = block4.Clone();
            blockToChange5.BlockTo = block2.Clone();
            blockToChange5.StudentId = student3.Id;
            await blockChangeService.AddAsync(blockToChange5);

            BlockChangeRequest blockToChange = new BlockChangeRequest();
            blockToChange.BlockFrom = block2.Clone();
            blockToChange.BlockTo = block1.Clone();
            blockToChange.StudentId = student3.Id;
            await blockChangeService.AddAsync(blockToChange);

            var exchange = await blockChangeService.FindExchange(blockToChange);
            exchange.StudentId.Should().Be(student1.Id);

            await blockChangeService.MakeExchangeAndDeleteRequests(exchange, blockToChange);


            blockChangeService.FindAllStudentRequests(student1.Id).Result.Count.Should().Be(0);
            blockChangeService.FindAllStudentRequests(student2.Id).Result.Count.Should().Be(2);
            blockChangeService.FindAllStudentRequests(student3.Id).Result.Count.Should().Be(1);
        }

    }
}
