using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.BackendTest;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using System;
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

            StudyGroupService grpsrvc = new StudyGroupService(DBSettings.Database);

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

            gr.Timetables.Add(tt);
            await grpsrvc.AddAsync(gr);

            st.StudyGroupId = gr.Id;

            await stSer.AddAsync(st);
            st = await stSer.FindByIdAsync(st.Id);

            st.Id.Should().NotBeEmpty(); // id was set?

            var studyGroup = await grpsrvc.FindByIdAsync(gr.Id);
            studyGroup.GroupName.Should().Be("5ZZS14");
            studyGroup.Timetables.First().Blocks.First().Day.Should().Be(Day.Thursday);
            studyGroup.Timetables.First().Blocks.First().Duration.Should().Be(2);
            studyGroup.Timetables.First().Blocks.First().StartHour.Should().Be(16);

            studyGroup.Timetables.First().Blocks.First().BlockType.Should().Be(BlockType.Lecture);
        }
    }
}
