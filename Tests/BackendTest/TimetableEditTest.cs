using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class TimetableEditTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;


        public TimetableEditTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
        }

        [Fact]
        public async Task EditBlockStudentTimetablePassed()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            StudentService stserv = new StudentService(database);
            Student student = new Student();
            student.Timetable = FakeTimetable.GetFakeTimetable();
            await stserv.AddAsync(student);

            var loadedStudent = await stserv.FindByIdAsync(student.Id);
            Block blckToAdd = new Block { BlockType = BlockType.Lecture, Day = Day.Friday, StartHour = 9 };

            loadedStudent.Timetable.AllBlocks.Count(x => x.Equals(blckToAdd)).Should().Be(0);
            //add new block
            loadedStudent.Timetable.AddNewBlock(blckToAdd);
            //save new block
            await stserv.UpdateStudentAsync(loadedStudent);
            //load from db
            Student updatedStudent = await stserv.FindByIdAsync(loadedStudent.Id);
            //test
            updatedStudent.Timetable.AllBlocks.Count(x => x.Equals(blckToAdd)).Should().Be(1);
            updatedStudent.Timetable.ContainsBlock(blckToAdd).Should().Be(true);


            //delete added block
            updatedStudent.Timetable.RemoveBlock(blckToAdd);
            //save deleted
            await stserv.UpdateStudentAsync(updatedStudent);
            //load from db
            updatedStudent = await stserv.FindByIdAsync(loadedStudent.Id);
            //test
            updatedStudent.Timetable.AllBlocks.Count(x => x.Equals(blckToAdd)).Should().Be(0);
            updatedStudent.Timetable.ContainsBlock(blckToAdd).Should().Be(false);
        }




    }
    public class FakeTimetable
    {
        public static Timetable GetFakeTimetable()
        {
            Timetable tt = new Timetable();

            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Excercise,
                Day = Day.Monday,
                Duration = 2,
                StartHour = 7
            });
            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Excercise,
                Day = Day.Monday,
                Duration = 2,
                StartHour = 10
            });
            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Excercise,
                Day = Day.Tuesday,
                Duration = 2,
                StartHour = 16
            });
            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Lecture,
                Day = Day.Wednesday,
                Duration = 2,
                StartHour = 12
            });
            tt.AddNewBlock(new Block
            {
                BlockType = BlockType.Lecture,
                Day = Day.Thursday,
                Duration = 2,
                StartHour = 11
            });
            return tt;
        }

    }
}

