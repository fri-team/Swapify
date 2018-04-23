using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class StudentServiceTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;

        public StudentServiceTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
        }

        [Fact]
        public async Task AddStudentTest()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            StudentService stSer = new StudentService(database);
            StudyGroupService grpsrvc = new StudyGroupService(database);
            Student st = new Student();
            Course cr = new Course() { CourseName = "DISS", Id = Guid.NewGuid() };
            StudyGroup gr = new StudyGroup() { GroupName = "5ZZS14" };
            Timetable tt = new Timetable();
            Block bl = new Block();
            bl.BlockType = BlockType.Lecture;
            bl.CourseId = cr.Id;
            bl.StartHour = 16;
            bl.Duration = 2;
            bl.Day = Day.Thursday;
            tt.Blocks.Add(bl);
            gr.Timetable = tt;


            await grpsrvc.AddAsync(gr);
            st.StudyGroupId = gr.Id;
            await stSer.AddAsync(st);

            st = await stSer.FindByIdAsync(st.Id);
            st.Id.Should().NotBeEmpty(); // id was set?
            var studyGroup = await grpsrvc.FindByIdAsync(gr.Id);
            studyGroup.GroupName.Should().Be("5ZZS14");
            studyGroup.Timetable.Blocks.First().Day.Should().Be(Day.Thursday);
            studyGroup.Timetable.Blocks.First().Duration.Should().Be(2);
            studyGroup.Timetable.Blocks.First().StartHour.Should().Be(16);
            studyGroup.Timetable.Blocks.First().BlockType.Should().Be(BlockType.Lecture);
        }
    }
}
