using FluentAssertions;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.APIWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using FRITeam.Swapify.APIWrapper.Objects;
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

            var expectedResult = new List<ScheduleHourContent>()
            {
                new ScheduleHourContent(0, 1, false,
                    LessonType.Lecture, "Jan Janech",
                    "RA045", "1181S",
                    "Metaprogramovanie", SubjectType.None),
                new ScheduleHourContent(2, 2, false,
                    LessonType.Laboratory, "Ida Stankovianska",
                    "RB053", "11BA31",
                    "pravdepodobnosť a štatistika", SubjectType.None)
            };

            parsed.Should().BeEquivalentTo(expectedResult);
        }
    }
}
