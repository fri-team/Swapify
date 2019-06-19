using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using MongoDB.Driver;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace BackendTest
{
    [Collection("Database collection")]
    public class StudentServiceTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;
        private readonly IMongoDatabase _database;
        private readonly Mock<ILogger<PersonalNumberService>> _loggerMock;
        private readonly Mock<ILogger<CourseService>> _loggerMockCourse;

        public StudentServiceTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
            _mongoFixture = mongoFixture;
            _loggerMock = new Mock<ILogger<PersonalNumberService>>();
            _loggerMockCourse = new Mock<ILogger<CourseService>>();
            _database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
        }


        [Fact]
        public async Task AssingTimetableToStudent()
        {
            var schoolScheduleProxy = new SchoolScheduleProxy();
            CourseService serviceCourse = new CourseService(_loggerMockCourse.Object, _database, schoolScheduleProxy);
            PersonalNumberService service = new PersonalNumberService(_loggerMock.Object, _database, schoolScheduleProxy, serviceCourse);

            PersonalNumber personalNumber = await service.GetPersonalNumberAsync("558188");
            Student student = new Student();
            student.Timetable = personalNumber.Timetable.Clone();
            student.PersonalNumber = personalNumber;

            var newBlock = new Block();
            var countShouldBe = personalNumber.Timetable.AllBlocks.Count;
            personalNumber.Timetable.AddNewBlock(newBlock);

            student.Timetable.AllBlocks.Count().Should().Be(countShouldBe); // check if timetable is copied

            var newBlockSt = new Block();
            student.Timetable.AddNewBlock(newBlockSt);
            student.Timetable.AllBlocks.Count().Should().Be(countShouldBe + 1); // +1 because of added block
        }

        [Fact]
        public async Task AddStudentTest()
        {
            var schoolScheduleProxy = new SchoolScheduleProxy();
            CourseService serviceCourse = new CourseService(_loggerMockCourse.Object, _database, schoolScheduleProxy);
            PersonalNumberService stnsrvc = new PersonalNumberService(_loggerMock.Object, _database, schoolScheduleProxy, serviceCourse);
            StudentService stSer = new StudentService(_database);
            
            Student st = new Student();
            Course cr = new Course() { CourseName = "DISS", Id = Guid.NewGuid() };
            PersonalNumber number = new PersonalNumber() { Number = "558188" };
            Timetable tt = new Timetable();
            Block bl = new Block();
            bl.BlockType = BlockType.Lecture;
            bl.CourseId = cr.Id;
            bl.StartHour = 16;
            bl.Duration = 2;
            bl.Day = Day.Thursday;
            tt.AddNewBlock(bl);
            number.Timetable = tt;


            await stnsrvc.AddAsync(number);
            st.PersonalNumber = number;
            await stSer.AddAsync(st);

            st = await stSer.FindByIdAsync(st.Id);
            st.Id.Should().NotBeEmpty(); // id was set?
            var personalNumber = await stnsrvc.FindByIdAsync(number.Id);
            personalNumber.Number.Should().Be("558188");
            personalNumber.Timetable.AllBlocks.First().Day.Should().Be(Day.Thursday);
            personalNumber.Timetable.AllBlocks.First().Duration.Should().Be(2);
            personalNumber.Timetable.AllBlocks.First().StartHour.Should().Be(16);
            personalNumber.Timetable.AllBlocks.First().BlockType.Should().Be(BlockType.Lecture);
        }


        [Fact]
        public async Task UpdateStudentTest()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            StudentService stSer = new StudentService(database);
            Student st = new Student();

            Block bl1 = new Block { Room = "room1" };
            Block bl2 = new Block { Room = "room2" };
            Block bl3 = new Block { Room = "room3" };

            st.Timetable = new Timetable();
            st.Timetable.AddNewBlock(bl1);
            st.Timetable.AddNewBlock(bl2);
            await stSer.AddAsync(st);
            st.Timetable.AllBlocks.Count().Should().Be(2);

            st = await stSer.FindByIdAsync(st.Id);
            st.Timetable.RemoveBlock(bl1).Should().Be(true);
            st.Timetable.AllBlocks.Count().Should().Be(1);
            st.Timetable.AllBlocks.FirstOrDefault().Room.Should().Be("room2");
            st.Timetable.AddNewBlock(bl3);

            await stSer.UpdateStudentAsync(st);
            st.Timetable.AllBlocks.Count().Should().Be(2);
            st.Timetable.AllBlocks.Any(x => x.Room == "room3").Should().Be(true);
            st.Timetable.AllBlocks.Any(x => x.Room == "room2").Should().Be(true);

        }
    }
}
