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
            tt.AddNewBlock(bl);
            gr.Timetable = tt;


            await grpsrvc.AddAsync(gr);
            st.StudyGroupId = gr.Id;
            await stSer.AddAsync(st);

            st = await stSer.FindByIdAsync(st.Id);
            st.Id.Should().NotBeEmpty(); // id was set?
            var studyGroup = await grpsrvc.FindByIdAsync(gr.Id);
            studyGroup.GroupName.Should().Be("5ZZS14");
            studyGroup.Timetable.AllBlocks.First().Day.Should().Be(Day.Thursday);
            studyGroup.Timetable.AllBlocks.First().Duration.Should().Be(2);
            studyGroup.Timetable.AllBlocks.First().StartHour.Should().Be(16);
            studyGroup.Timetable.AllBlocks.First().BlockType.Should().Be(BlockType.Lecture);
        }


        [Fact]
        public async Task UpdateStudentTest()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            StudentService stSer = new StudentService(database);
            Student st = new Student();

            Block bl1 = new Block() { Room = "room1" };
            Block bl2 = new Block() { Room = "room2" };
            Block bl3 = new Block() { Room = "room3" };

            st.Timetable = new Timetable();
            st.Timetable.AddNewBlock(bl1);
            st.Timetable.AddNewBlock(bl2);
            await stSer.AddAsync(st);
            st.Timetable.AllBlocks.Count(x => x.Id == Guid.Empty).Should().Be(0);

            st = await stSer.FindByIdAsync(st.Id);
            st.Timetable.RemoveBlock(bl1.Id).Should().Be(true);
            bl2.Room = "newroom";
            st.Timetable.UpdateBlock(bl2);
            st.Timetable.AddNewBlock(bl3);

            await stSer.UpdateStudentAsync(st);
            st.Timetable.AllBlocks.FirstOrDefault(x => x.Id.Equals(bl2.Id)).Room.Should().Be("newroom");
            st.Timetable.AllBlocks.Count(x => x.Id == Guid.Empty).Should().Be(0);
            st.Timetable.AllBlocks.Count.Should().Be(2);
        }
    }
}
