using FluentAssertions;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.APIWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BackendTest
{

    public class ResponceParserTest
    {

        [Fact]
        public void ParseResponce_BadInputParameter()
        {
            string input = "{\"report\":\"Nena\u0161li sa \\u017eiadne bloky pre rozvrh.\",\"ScheduleContent\":[]}";

            Action act = () => ResponseParser.ParseResponse(input);
            Assert.Throws<ArgumentException>(act);

        }

        [Fact]
        public void ParseResponce_ParseCorrectBlocks()
        {
            var input = @"
                {
                'report': null,
                'ScheduleContent': [
                        {
                            'b': '1',
                            'tu': 'P',
                            'u': 'Jan Janech',
                            'r': 'RA045',
                            's': 'Metaprogramovanie',
                            'k': '1181S',
                            'dw':'1'
                        },
                        {
                            'b': '2',
                            'tu': 'L',
                            'u': 'Ida Stankovianska',
                            'r': 'RB053',
                            's': 'pravdepodobnosť a štatistika',
                            'k': '11BA31',
                            'dw': '3'
                        }
                    ]
                }";

            var parsed = ResponseParser.ParseResponse(input);

            parsed.DaysInWeek[0].BlocksInDay[0].LessonType.Should().Be(LessonType.Lecture);

            parsed.DaysInWeek[2].BlocksInDay[0].BlockNumber.Should().Be(2);
            parsed.DaysInWeek[2].BlocksInDay[0].LessonType.Should().Be(LessonType.Laboratory);
            parsed.DaysInWeek[2].BlocksInDay[0].RoomName.Should().Be("RB053");
            parsed.DaysInWeek[2].BlocksInDay[0].CourseName.Should().Be("pravdepodobnosť a štatistika");
            parsed.DaysInWeek[2].BlocksInDay[0].CourseShortcut.Should().Be("11BA31");
            parsed.DaysInWeek[2].BlocksInDay[0].TeacherName.Should().Be("Ida Stankovianska");
        }
    }
}
