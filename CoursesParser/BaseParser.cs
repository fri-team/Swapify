using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoursesParser
{
    public class BaseParser
    {
        private HtmlDocument _document;
        private HtmlNode _selectFaculties;
        private HtmlNode _selectTown;
        private HtmlNode _selectStudyType;
        private HtmlNode _selectStudyYear;
        private HtmlNode _selectFieldOfStudy;
        private HtmlNode _selectFieldOfStudyDetailed;
        private HtmlNode _selectPlansTable;
        private string _facId;
        private string _townId;
        private string _studyTypeId;
        private string _studyYearId;
        private string _fieldOfStudyId;
        private string _fieldOfStudyDetailedId;
        private List<string> _allCourses;

        public BaseParser()
        {
            var url = "https://vzdelavanie.uniza.sk/vzdelavanie/plany.php";
            var web = new HtmlWeb();
            _document = web.Load(url);

            _selectFaculties = _document.GetElementbyId("f");
            _selectTown = _document.GetElementbyId("t");
            _selectStudyType = _document.GetElementbyId("m");
            _selectStudyYear = _document.GetElementbyId("r");
            _selectFieldOfStudy = _document.GetElementbyId("o");
            _selectFieldOfStudyDetailed = _document.GetElementbyId("z");
            _selectPlansTable = _document.GetElementbyId("plany").ChildNodes["table"];

        }


        public void ParseFaculties()
        {
            
            foreach (var facultyOption in _selectFaculties.ChildNodes)
            {
                _facId = facultyOption.Attributes["value"].Value;
                foreach (var town in _selectTown.ChildNodes)
                {
                    _townId = town.Attributes["value"].Value;
                    foreach(var studyType in _selectStudyType.ChildNodes)
                    {
                        _studyTypeId = studyType.Attributes["value"].Value;
                        foreach (var studyYear in _selectStudyYear.ChildNodes)
                        {
                            _studyYearId = studyYear.Attributes["value"].Value;
                            foreach (var fieldOfStudy in _selectFieldOfStudy.ChildNodes)
                            {
                                _fieldOfStudyId = fieldOfStudy.Attributes["value"].Value;
                                foreach (var fieldOfStudyDetailed in _selectFieldOfStudyDetailed.ChildNodes)
                                {
                                    _fieldOfStudyDetailedId = fieldOfStudyDetailed.Attributes["value"].Value;
                                    DownloadAndSaveCourses(6);
                                }
                                DownloadAndSaveCourses(5);
                            }
                            DownloadAndSaveCourses(4);
                        }
                        DownloadAndSaveCourses(3);
                    }
                    DownloadAndSaveCourses(2);
                }
                DownloadAndSaveCourses(1);
            }

            
        }

        private void DownloadAndSaveCourses(int level)
        {
            var courses = DownloadCourses(_facId, _townId, _studyTypeId, _studyYearId,
                                          _fieldOfStudyId, _fieldOfStudyDetailedId, level.ToString());
            _allCourses.AddRange(courses);
        }


        private List<string> DownloadCourses(string f,string t,string m,string r,
                                             string o,string z,string c)
        {
            List<string> ret = new List<string>();
            var url = $"http://vzdelavanie.uniza.sk/vzdelavanie/plany.php?f={f}&t={t}&m={m}&r={r}&o={o}&z={z}&c={c}";
            var web = new HtmlWeb();
            _document = web.Load(url);


            return ret;
        }

        public void ParseCourses(HtmlNode htmlTable)
        {
            var rows = htmlTable.ChildNodes["tbody"].SelectNodes("tr");
            foreach (var x in rows)
            {
                var course = x.ChildNodes["td"]?.ChildNodes["a"];
                if (course != null)
                {
                    Debug.WriteLine(course.InnerText);
                }
            }
        }
    }
}
