using CoursesParser;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BackendTest
{
    public class CourseParserTest
    {
        [Fact]
        private void ParseFaculties()
        {
            BaseParser parser = new BaseParser();
            parser.ParseFaculties();
        }
    }
}
