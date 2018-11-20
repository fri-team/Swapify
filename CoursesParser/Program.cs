using System;
using System.IO;
using System.Linq;

namespace CoursesParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            BaseParser parser = new BaseParser();
            //download from elearning - it can take few minutes!
            int id = 1;
            var allCourses = parser.ParseFaculties().Select(x =>
            new
            {
                Id = id++,
                CourseCode = parser.SplitCodeAndName(x).Item1,
                CourseName = parser.SplitCodeAndName(x).Item2
            });


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(allCourses);
            var actDate = DateTime.Now;
            File.WriteAllText($"courses_{actDate.Year}_{actDate.Month}_{actDate.Day}.json", json);

        }
    }
}
