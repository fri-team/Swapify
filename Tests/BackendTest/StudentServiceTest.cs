using FluentAssertions;
using FRITeam.Swapify.Backend;
using System;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using MongoDB.Driver;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.SwapifyBase.Entities;
using Microsoft.Extensions.Options;
using FRITeam.Swapify.SwapifyBase.Settings.ProxySettings;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;

namespace BackendTest
{
    [Collection("Database collection")]
    public class StudentServiceTest : IClassFixture<Mongo2GoFixture>
    {        
        private readonly Mongo2GoFixture _mongoFixture;
        private readonly IMongoDatabase _database;
        private readonly Mock<ILogger<SchoolScheduleProxy>> _loggerMockSchedule;
        private readonly Mock<ILogger<CourseService>> _loggerMockCourse;

        public StudentServiceTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
            _mongoFixture = mongoFixture;
            _loggerMockSchedule = new Mock<ILogger<SchoolScheduleProxy>>();
            _loggerMockCourse = new Mock<ILogger<CourseService>>();
            _database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
        }

        [Fact]
        public async Task AssingTimetableToStudent()
        {
            var options = GetProxyOptions();
            var schoolScheduleProxy = new SchoolScheduleProxy(options);
            var schoolCourseProxy = new SchoolCourseProxy(options);
            CourseService serviceCourse = new(_loggerMockCourse.Object, _database, schoolScheduleProxy, schoolCourseProxy);            

            var timetable = await schoolScheduleProxy.GetByPersonalNumber("559841");
            TimetableData ttData = new()
            {
                PersonalNumber = "559841",
                Timetable = await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(timetable, serviceCourse)
            };

            var newBlock = new Block();
            var countShouldBe = ttData.Timetable.AllBlocks.Count;
            ttData.Timetable.AddNewBlock(newBlock);

            ttData.Timetable.AllBlocks.Count().Should().Be(countShouldBe + 1);

            var newBlockSt = new Block();
            countShouldBe = ttData.Timetable.AllBlocks.Count;
            ttData.Timetable.AddNewBlock(newBlockSt);

            ttData.Timetable.AllBlocks.Count().Should().Be(countShouldBe + 1);
        }

        [Fact]
        public async Task AddStudentTest()
        {
            var options = GetProxyOptions();
            var schoolScheduleProxy = new SchoolScheduleProxy(options);
            var schoolCourseProxy = new SchoolCourseProxy(options);
            CourseService serviceCourse = new(_loggerMockCourse.Object, _database, schoolScheduleProxy, schoolCourseProxy);            
            TimetableDataService stSer = new(_database);

            var timetable = await schoolScheduleProxy.GetByPersonalNumber("559841");
            TimetableData ttData = new()
            {
                PersonalNumber = "559841",
                Timetable = await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(timetable, serviceCourse)
            };
            Course cr = new() { CourseName = "DISS", Id = Guid.NewGuid() };
            Block bl = new()
            {
                BlockType = BlockType.Lecture,
                CourseId = cr.Id,
                StartHour = 16,
                Duration = 2,
                Day = Day.Thursday
            };

            ttData.Timetable.AddNewBlock(bl);

            await stSer.AddAsync(ttData);

            ttData = await stSer.FindByIdAsync(ttData.Id);
            ttData.Id.Should().NotBeEmpty();
            ttData.PersonalNumber.Should().Be("559841");
            ttData.Timetable.AllBlocks.Last().Day.Should().Be(Day.Thursday);
            ttData.Timetable.AllBlocks.Last().Duration.Should().Be(2);
            ttData.Timetable.AllBlocks.Last().StartHour.Should().Be(16);
            ttData.Timetable.AllBlocks.Last().BlockType.Should().Be(BlockType.Lecture);
        }

        [Fact]
        public async Task UpdateStudentTest()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            TimetableDataService timetableDataService = new(database);
            TimetableData ttData = new();

            Block bl1 = new() { Room = "room1" };
            Block bl2 = new() { Room = "room2" };
            Block bl3 = new() { Room = "room3" };

            ttData.Timetable = new Timetable(Semester.GetSemester());
            ttData.Timetable.AddNewBlock(bl1);
            ttData.Timetable.AddNewBlock(bl2);
            await timetableDataService.AddAsync(ttData);
            ttData.Timetable.AllBlocks.Count().Should().Be(2);

            ttData = await timetableDataService.FindByIdAsync(ttData.Id);
            ttData.Timetable.RemoveBlock(bl1.BlockId).Should().Be(true);
            ttData.Timetable.AllBlocks.Count().Should().Be(1);
            ttData.Timetable.AllBlocks.FirstOrDefault().Room.Should().Be("room2");
            ttData.Timetable.AddNewBlock(bl3);

            await timetableDataService.UpdateTimetableDataAsync(ttData);
            ttData.Timetable.AllBlocks.Count().Should().Be(2);
            ttData.Timetable.AllBlocks.Any(x => x.Room == "room3").Should().Be(true);
            ttData.Timetable.AllBlocks.Any(x => x.Room == "room2").Should().Be(true);
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
