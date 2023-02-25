using FluentAssertions;
using FRITeam.Swapify.Backend;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using Microsoft.Extensions.Options;
using FRITeam.Swapify.SwapifyBase.Settings.ProxySettings;

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
            TimetableDataService timetableDataService = new TimetableDataService(database);
            BlockChangesService blockChangeService = new BlockChangesService(database);
            var options = GetProxyOptions();
            var schoolScheduleProxy = new SchoolScheduleProxy(options);
            var schoolCourseProxy = new SchoolCourseProxy(options);
            CourseService courseService = new CourseService(_loggerMockCourse.Object, database, schoolScheduleProxy, schoolCourseProxy);

            Course course = await CreateAndAddCourse("Programovanie", "11111", courseService);
            Course course2 = await CreateAndAddCourse("Programovanie", "11111", courseService);


            Block block1 = CreateBlock(BlockType.Laboratory, Day.Monday, 2, 7, course.Id);
            Block block2 = CreateBlock(BlockType.Laboratory, Day.Wednesday, 2, 10, course.Id);
            Block block3 = CreateBlock(BlockType.Laboratory, Day.Tuesday, 2, 15, course.Id);
            Block block4 = CreateBlock(BlockType.Laboratory, Day.Friday, 2, 18, course2.Id);

            TimetableData ttData1 = new TimetableData();
            TimetableData ttData2 = new TimetableData();
            TimetableData ttData3 = new TimetableData();
            await timetableDataService.AddAsync(ttData1);
            await timetableDataService.AddAsync(ttData2);
            await timetableDataService.AddAsync(ttData3);

            BlockChangeRequest blockToChange1 = CreateBlockChangeRequest(block1, block2, ttData1.Id);
            BlockChangeRequest blockToChange2 = CreateBlockChangeRequest(block1, block3, ttData1.Id);
            BlockChangeRequest blockToChange3 = CreateBlockChangeRequest(block1, block2, ttData2.Id);
            BlockChangeRequest blockToChange4 = CreateBlockChangeRequest(block1, block3, ttData2.Id);
            BlockChangeRequest blockToChange5 = CreateBlockChangeRequest(block4, block2, ttData3.Id);
            BlockChangeRequest blockToChange = CreateBlockChangeRequest(block2, block1, ttData3.Id);
            ValueTuple<BlockChangeRequest, BlockChangeRequest> result = new ValueTuple<BlockChangeRequest, BlockChangeRequest>();

            result = (null, null);
            (await blockChangeService.AddAndFindMatch(blockToChange1)).Should().Equals(result);
            (await blockChangeService.AddAndFindMatch(blockToChange2)).Should().Equals(result);
            (await blockChangeService.AddAndFindMatch(blockToChange3)).Should().Equals(result);
            (await blockChangeService.AddAndFindMatch(blockToChange4)).Should().Equals(result);
            (await blockChangeService.AddAndFindMatch(blockToChange5)).Should().Equals(result);

            result = (blockToChange, blockToChange1);
            (await blockChangeService.AddAndFindMatch(blockToChange)).Should().Equals(result);

            blockChangeService.FindAllStudentRequests(ttData1.Id).Result.Count.Should().Be(1);
            blockChangeService.FindAllStudentRequests(ttData2.Id).Result.Count.Should().Be(2);
            blockChangeService.FindAllStudentRequests(ttData3.Id).Result.Count.Should().Be(2);

            blockChangeService.FindWaitingStudentRequests(ttData1.Id).Result.Count.Should().Be(0);
            blockChangeService.FindWaitingStudentRequests(ttData2.Id).Result.Count.Should().Be(2);
            blockChangeService.FindWaitingStudentRequests(ttData3.Id).Result.Count.Should().Be(1);

            BlockChangeRequest blockToChange6 = CreateBlockChangeRequest(block3, block2, ttData1.Id);
            (await blockChangeService.AddAndFindMatch(blockToChange6)).Should().Be((null, null));
            blockChangeService.FindWaitingStudentRequests(ttData1.Id).Result.Count.Should().Be(1);
            blockChangeService.FindWaitingStudentRequests(ttData2.Id).Result.Count.Should().Be(2);
            blockChangeService.FindWaitingStudentRequests(ttData3.Id).Result.Count.Should().Be(1);
        }


        [Fact]
        public async Task CancelExchangeTest()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            TimetableDataService studentSrv = new TimetableDataService(database);
            BlockChangesService blockChangeService = new BlockChangesService(database);
            var options = GetProxyOptions();
            var schoolScheduleProxy = new SchoolScheduleProxy(options);
            var schoolCourseProxy = new SchoolCourseProxy(options);
            CourseService courseService = new CourseService(_loggerMockCourse.Object, database, schoolScheduleProxy, schoolCourseProxy);

            Course course = await CreateAndAddCourse("Programovanie", "11111", courseService);

            Block block1 = CreateBlock(BlockType.Laboratory, Day.Monday, 2, 7, course.Id);
            Block block2 = CreateBlock(BlockType.Laboratory, Day.Wednesday, 2, 10, course.Id);
            Block block3 = CreateBlock(BlockType.Laboratory, Day.Wednesday, 2, 8, course.Id);

            TimetableData student = new TimetableData();
            await studentSrv.AddAsync(student);

            BlockChangeRequest blockToChange = CreateBlockChangeRequest(block2, block1, student.Id);
            BlockChangeRequest blockToChange1 = CreateBlockChangeRequest(block3, block1, student.Id);

            (await blockChangeService.AddAndFindMatch(blockToChange)).Should().Be((null, null));
            (await blockChangeService.AddAndFindMatch(blockToChange1)).Should().Be((null, null));

            blockChangeService.FindWaitingStudentRequests(student.Id).Result.Count.Should().Be(2);
            (await blockChangeService.CancelExchangeRequest(blockToChange1)).Should().Be(true);
            blockChangeService.FindWaitingStudentRequests(student.Id).Result.Count.Should().Be(1);
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

        private BlockChangeRequest CreateBlockChangeRequest(Block blockFrom, Block blockTo, Guid timetableId)
        {
            BlockChangeRequest blockToChange = new BlockChangeRequest();
            blockToChange.DateOfCreation = DateTime.Now;
            blockToChange.BlockFrom = blockFrom.Clone();
            blockToChange.BlockTo = blockTo.Clone();
            blockToChange.TimetableId = timetableId;
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

        private IOptions<ProxySettings> GetProxyOptions()
        {
            var settings = new ProxySettings()
            {
                Base_URL = "https://nic.uniza.sk/webservices",
                CourseContentURL = "getUnizaScheduleType4.php",
                ScheduleContentURL = "getUnizaScheduleContent.php"
            };
            return Options.Create(settings);
        }
    }
}
