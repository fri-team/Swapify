using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FRITeam.Swapify.APIWrapper;

namespace BackendTest
{
    [Collection("Database collection")]
    public class BlockChangeRequestTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;
        private readonly Mock<ILogger<CourseService>> _loggerMockCourse;

        public BlockChangeRequestTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
            _loggerMockCourse = new Mock<ILogger<CourseService>>();

        }

        [Fact]
        public async Task ExchangeRequests_ExchangingRequests_ExchangedRequests()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            StudentService studentSrv = new StudentService(database);
            BlockChangesService blockChangeService = new BlockChangesService(database);
            var schoolScheduleProxy = new SchoolScheduleProxy();
            CourseService courseService = new CourseService(_loggerMockCourse.Object, database, schoolScheduleProxy);

            Course course = await CreateAndAddCourse("Programovanie", "11111", courseService);
            Course course2 = await CreateAndAddCourse("Programovanie", "11111", courseService);


            Block block1 = CreateBlock(BlockType.Laboratory, Day.Monday, 2, 7, course.Id);
            Block block2 = CreateBlock(BlockType.Laboratory, Day.Wednesday, 2, 10, course.Id);
            Block block3 = CreateBlock(BlockType.Laboratory, Day.Tuesday, 2, 15, course.Id);
            Block block4 = CreateBlock(BlockType.Laboratory, Day.Friday, 2, 18, course2.Id);

            Student student1 = new Student();
            Student student2 = new Student();
            Student student3 = new Student();
            await studentSrv.AddAsync(student1);
            await studentSrv.AddAsync(student2);
            await studentSrv.AddAsync(student3);

            BlockChangeRequest blockToChange1 = CreateBlockChangeRequest(block1, block2, student1.Id);
            BlockChangeRequest blockToChange2 = CreateBlockChangeRequest(block1, block3, student1.Id);
            BlockChangeRequest blockToChange3 = CreateBlockChangeRequest(block1, block2, student2.Id);
            BlockChangeRequest blockToChange4 = CreateBlockChangeRequest(block1, block3, student2.Id);
            BlockChangeRequest blockToChange5 = CreateBlockChangeRequest(block4, block2, student3.Id);
            BlockChangeRequest blockToChange = CreateBlockChangeRequest(block2, block1, student3.Id);

            (await blockChangeService.AddAndFindMatch(blockToChange1)).Should().Be(false);
            (await blockChangeService.AddAndFindMatch(blockToChange2)).Should().Be(false);
            (await blockChangeService.AddAndFindMatch(blockToChange3)).Should().Be(false);
            (await blockChangeService.AddAndFindMatch(blockToChange4)).Should().Be(false);
            (await blockChangeService.AddAndFindMatch(blockToChange5)).Should().Be(false);
            (await blockChangeService.AddAndFindMatch(blockToChange)).Should().Be(true);

            blockChangeService.FindAllStudentRequests(student1.Id).Result.Count.Should().Be(0);
            blockChangeService.FindAllStudentRequests(student2.Id).Result.Count.Should().Be(2);
            blockChangeService.FindAllStudentRequests(student3.Id).Result.Count.Should().Be(1);

            BlockChangeRequest blockToChange6 = CreateBlockChangeRequest(block3, block2, student1.Id);
            (await blockChangeService.AddAndFindMatch(blockToChange6)).Should().Be(false);
            blockChangeService.FindWaitingStudentRequests(student1.Id).Result.Count.Should().Be(1);
            blockChangeService.FindWaitingStudentRequests(student2.Id).Result.Count.Should().Be(2);
            blockChangeService.FindWaitingStudentRequests(student3.Id).Result.Count.Should().Be(1);

        }

        private Block CreateBlock(BlockType blockType, Day day, byte duration, byte startHour, Guid courseId)
        {
            return new Block()
            {
                BlockType = blockType,
                Day = day,
                Duration = duration,
                StartHour = startHour,
                CourseId = courseId
            };
        }

        private BlockChangeRequest CreateBlockChangeRequest(Block blockFrom, Block blockTo, Guid studentId)
        {
            BlockChangeRequest blockToChange = new BlockChangeRequest();
            blockToChange.DateOfCreation = DateTime.Now;
            blockToChange.BlockFrom = blockFrom.Clone();
            blockToChange.BlockTo = blockTo.Clone();
            blockToChange.StudentId = studentId;
            return blockToChange;

        }

        private async Task<Course> CreateAndAddCourse(string name, string code, CourseService service)
        {
            var course = new Course();
            course.CourseName = name;
            course.CourseCode = code;
            await service.AddAsync(course);
            return course;
        }
    }
}
