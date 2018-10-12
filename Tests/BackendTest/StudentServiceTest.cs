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
        public async Task AssingTimetableToStudent()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            StudyGroupService grpsrvc = new StudyGroupService(database);
            CourseService crsrv = new CourseService(database);
            ISchoolScheduleProxy proxy = new FakeProxy();

            StudyGroup sg = await grpsrvc.GetStudyGroupAsync("5ZI001", crsrv, proxy);
            Student s = new Student();
            s.Timetable = sg.Timetable.Clone();
            s.StudyGroupId = sg.Id;

            var newBlock = new Block();

            sg.Timetable.AddNewBlock(newBlock);

            s.Timetable.GetBlock(newBlock.Id).Should().BeNull();

            var newBlockSt = new Block();
            s.Timetable.AddNewBlock(newBlockSt);

            sg.Timetable.GetBlock(newBlockSt.Id).Should().BeNull();
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
    }
}
