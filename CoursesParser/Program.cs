using FRITeam.Swapify.Backend.CourseParser;
using System;
using System.Collections.Generic;
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
            List<CourseItem> allCourses = parser.ParseFaculties();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(allCourses);
            var actDate = DateTime.Now;
            File.WriteAllText($"courses_{actDate.Year}_{actDate.Month}_{actDate.Day}.json", json);

        }
    }
}
