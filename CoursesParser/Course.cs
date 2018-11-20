using CoursesParser.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesParser
{
    public class Course
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Faculty { get; set; }
        public string Town { get; set; }
        public string StudyType { get; set; }
        public string YearOfStudy { get; set; }
        public string StudyOfField { get; set; }
        public string DetailedStudyOfField { get; set; }
    }
}
