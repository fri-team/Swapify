using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using Xunit;

namespace BackendTest
{
    public class ScheduleHourTest
    {

        [Fact]
        public void CorrectSortingOfBlocks()
        {
            var meta = new ScheduleHourContent(1, false, LessonType.Laboratory, "Jan Janech", "RA045", "1181S",
                "Metaprogramovanie", SubjectType.Elective, new List<string>());
            var pas = new ScheduleHourContent(5, false, LessonType.Excercise, "Ida Stankovianska", "RB053", "11BA31",
                "Pravdepodobnosť a štatistika", SubjectType.Compulsory, new List<string>());
            var pas1 = new ScheduleHourContent(6, false, LessonType.Excercise, "Ida Stankovianska", "RB053", "11BA31",
                "Pravdepodobnosť a štatistika", SubjectType.Compulsory, new List<string>());
            var prog = new ScheduleHourContent(7, false, LessonType.Excercise, "Stefan Toth", "RA333", "11111",
                "Programovanie", SubjectType.Compulsory, new List<string>());
            var telesna = new ScheduleHourContent(7, false, LessonType.Excercise, "Dusan Giba", "T-1", "12121",
                "Telesna", SubjectType.Optional, new List<string>());

            List<ScheduleHourContent> list = new List<ScheduleHourContent>();
            telesna.GetIndexOfSameBlockInList(list).Should().Be(-2);

            list.Add(meta);
            list.Add(pas);

            pas1.GetIndexOfSameBlockInList(list).Should().Be(1);
            prog.GetIndexOfSameBlockInList(list).Should().Be(-1);
        }
    }
}
