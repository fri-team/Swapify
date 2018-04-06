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
        public void ParseResponce_ParseEmptyAndCorrectBlock()
        {
            var input = @"
                {
                'report': null,
                'ScheduleContent': [
                    [
                        {
                            'b': 0,
                            't': '',
                            'u': '',
                            'r': '',
                            's': '',
                            'k': '',
                            'g': '',
                            'p': ''
                        },
                        {
                            'b': 0,
                            't': 'L',
                            'u': 'Ida Stankovianska',
                            'r': 'RB053',
                            's': null,
                            'k': 'pravdepodobnosť a štatistika',
                            'g': '5ZK011, 5ZZS11, 5ZZS12, 5ZZS13, 5ZZS14',
                            'p': '1'
                        }
                    ]   
                    ]
                }";

            var parsed = ResponseParser.ParseResponse(input);

            parsed.DaysInWeek[0].BlocksInDay[0].Should().BeNull();
            parsed.DaysInWeek[0].BlocksInDay[1].BlockNumber.Should().Be(1);
            parsed.DaysInWeek[0].BlocksInDay[1].IsBlocked.Should().Be(false);
            parsed.DaysInWeek[0].BlocksInDay[1].LessonType.Should().Be(LessonType.Laboratory);
            parsed.DaysInWeek[0].BlocksInDay[1].RoomName.Should().Be("RB053");
            parsed.DaysInWeek[0].BlocksInDay[1].CourseName.Should().Be("pravdepodobnosť a štatistika");
            parsed.DaysInWeek[0].BlocksInDay[1].CourseShortcut.Should().Be("");
            parsed.DaysInWeek[0].BlocksInDay[1].SubjectType.Should().Be(SubjectType.Compulsory);
            parsed.DaysInWeek[0].BlocksInDay[1].TeacherName.Should().Be("Ida Stankovianska");
            parsed.DaysInWeek[0].BlocksInDay[1].StudyGroups.Should().Contain("5ZK011");
            parsed.DaysInWeek[0].BlocksInDay[1].StudyGroups.Should().Contain("5ZZS11");
            parsed.DaysInWeek[0].BlocksInDay[1].StudyGroups.Should().Contain("5ZZS12");
            parsed.DaysInWeek[0].BlocksInDay[1].StudyGroups.Should().Contain("5ZZS13");
            parsed.DaysInWeek[0].BlocksInDay[1].StudyGroups.Should().Contain("5ZZS14");
        }
    }
}
