using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.BackendTest;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BackendTest
{
    public class StudentServiceTest
    {
        [Fact]
        public async Task AddStudentTest()
        {
            DBSettings.InitDBSettings(MongoRunnerType.Test);
            StudentService stSer = new StudentService(DBSettings.Database);
            Student st = new Student();

            Course cr = new Course() { CourseName = "DISS" };
            StudyGroup gr = new StudyGroup() { GroupName = "5ZZS14" };
            Timetable tt = new Timetable();

            TimeSlot ts = new TimeSlot() { Day = eDay.Thursday, StartHour = 15};
            TimeSlot ts1 = new TimeSlot() { Day = eDay.Thursday, StartHour = 16 };
            
            Block bl = new Block();
            bl.BlockType = eBlockType.Lecture;
            bl.Course = cr;
            bl.TimeSlots.Add(ts);
            bl.TimeSlots.Add(ts1);

            tt.Blocks.Add(bl);

            gr.Timetables.Add(tt);
            st.StudyGroup = gr;

            await stSer.AddStudentAsync(st);
            st = await stSer.FindStudentById(st.Id.ToString());

            st.Id.Should().NotBeEmpty(); // id was set?

            st.StudyGroup.GroupName.Should().Be("5ZZS14");
            st.StudyGroup.Timetables.First().Blocks.First().TimeSlots[0].StartHour.Should().Be(15);
            st.StudyGroup.Timetables.First().Blocks.First().TimeSlots[1].StartHour.Should().Be(16);

            st.StudyGroup.Timetables.First().Blocks.First().TimeSlots[1].Day.Should().Be(eDay.Thursday);
            st.StudyGroup.Timetables.First().Blocks.First().BlockType.Should().Be(eBlockType.Lecture);
            st.StudyGroup.Timetables.First().Blocks.First().Course.CourseName.Should().Be("DISS");
        }
    }
}
