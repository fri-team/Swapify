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
using FRITeam.Swapify.Backend.Converter;

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
            var schoolScheduleProxy = new SchoolScheduleProxy();
            CourseService serviceCourse = new CourseService(_loggerMockCourse.Object, _database, schoolScheduleProxy);
            SchoolScheduleProxy serviceSchedule = new SchoolScheduleProxy();
            
            Student student = new Student();
            var timetable = serviceSchedule.GetByPersonalNumber("558188");
            student.Timetable = await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(timetable, serviceCourse);
            student.PersonalNumber = "558188";

            var newBlock = new Block();
            var countShouldBe = student.Timetable.AllBlocks.Count;
            student.Timetable.AddNewBlock(newBlock);

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
            SchoolScheduleProxy serviceSchedule = new SchoolScheduleProxy();
            StudentService stSer = new StudentService(_database);

            var timetable = serviceSchedule.GetByPersonalNumber("558188");
            Student st = new Student
            {
                PersonalNumber = "558188",
                Timetable = await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(timetable, serviceCourse)
        };
            Course cr = new Course() { CourseName = "DISS", Id = Guid.NewGuid() };
            Timetable tt = new Timetable();
            Block bl = new Block
            {
                BlockType = BlockType.Lecture,
                CourseId = cr.Id,
                StartHour = 16,
                Duration = 2,
                Day = Day.Thursday
            };
            tt.AddNewBlock(bl);
            
            await stSer.AddAsync(st);

            st = await stSer.FindByIdAsync(st.Id);
            st.Id.Should().NotBeEmpty(); // id was set?
            st.PersonalNumber.Should().Be("558188");
            st.Timetable.AllBlocks.First().Day.Should().Be(Day.Thursday);
            st.Timetable.AllBlocks.First().Duration.Should().Be(2);
            st.Timetable.AllBlocks.First().StartHour.Should().Be(16);
            st.Timetable.AllBlocks.First().BlockType.Should().Be(BlockType.Lecture);
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
