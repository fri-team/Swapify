using CoursesParser.Enums;
using FRITeam.Swapify.Backend.CourseParser;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace CoursesParser
{
    public class BaseParser
    {
        private const string Url = "https://vzdelavanie.uniza.sk/vzdelavanie/plany.php";
        private readonly HtmlDocument _document;
        private readonly HtmlNode _selectFaculties;
        private HtmlNode _selectTown;
        private HtmlNode _selectStudyType;
        private HtmlNode _selectStudyYear;
        private HtmlNode _selectFieldOfStudy;
        private HtmlNode _selectFieldOfStudyDetailed;

        private HtmlNode _facAct;
        private HtmlNode _townAct;
        private HtmlNode _studyTypeAct;
        private HtmlNode _studyYearAct;
        private HtmlNode _fieldOfStudyAct;
        private HtmlNode _fieldOfStudyDetailedAct;
        private readonly List<CourseItem> _allCourses;
        private readonly Encoding _encoding; 

        public BaseParser()
        {
            _encoding = CodePagesEncodingProvider.Instance.GetEncoding(1250);

            var web = new HtmlWeb();
            web.AutoDetectEncoding = false;
            web.OverrideEncoding = _encoding;

            _allCourses = new List<CourseItem>(5000);
            _document = web.Load(Url);
            _document.OptionDefaultStreamEncoding = _encoding;

            _selectFaculties = _document.GetElementbyId("f");
            _selectFaculties.RemoveChild(_selectFaculties.FirstChild);
            _selectTown = _document.GetElementbyId("t");
            _selectTown.RemoveChild(_selectTown.FirstChild);
            _selectStudyType = _document.GetElementbyId("m");
            _selectStudyType.RemoveChild(_selectStudyType.FirstChild);
            _selectStudyYear = _document.GetElementbyId("r");
            _selectStudyYear.RemoveChild(_selectStudyYear.FirstChild);
            //_selectFieldOfStudy = _document.GetElementbyId("o");
            _selectFieldOfStudyDetailed = _document.GetElementbyId("z");
            _selectFieldOfStudyDetailed.RemoveChild(_selectFieldOfStudyDetailed.FirstChild);
        }


        public List<CourseItem> ParseFaculties()
        {
            foreach (var facultyOption in _selectFaculties.ChildNodes)
            {
                _facAct = facultyOption;
                Console.WriteLine(facultyOption.InnerText);
                DownloadAndSaveCourses(ChangeLevel.FromFaculty);
                foreach (var town in _selectTown.ChildNodes)
                {
                    _townAct = town;
                    Console.WriteLine(town.InnerText);
                    DownloadAndSaveCourses(ChangeLevel.FromTown);
                    foreach (var studyType in _selectStudyType.ChildNodes)
                    {
                        _studyTypeAct = studyType;
                        Console.WriteLine(studyType.InnerText);
                        DownloadAndSaveCourses(ChangeLevel.FromStudyType);
                        foreach (var studyYear in _selectStudyYear.ChildNodes)
                        {
                            _studyYearAct = studyYear;
                            Console.WriteLine(studyYear.InnerText);
                            DownloadAndSaveCourses(ChangeLevel.FromStudyYear); 
                            foreach (var fieldOfStudyDetailed in _selectFieldOfStudyDetailed.ChildNodes)
                            {
                                _fieldOfStudyDetailedAct = fieldOfStudyDetailed;
                                DownloadAndSaveCourses(ChangeLevel.FromDetailedFieldOfStudy);
                            }
                                                     
                        }
                    }
                }
            }
            return _allCourses;
        }

        private void DownloadAndSaveCourses(ChangeLevel level)
        {
            if (_facAct.Attributes["value"]?.Value == null ||
                _townAct?.Attributes["value"]?.Value == null ||
                _studyTypeAct?.Attributes["value"]?.Value == null ||
                _studyYearAct?.Attributes["value"]?.Value == null ||                
                _fieldOfStudyDetailedAct?.Attributes["value"]?.Value == null)
            {
                return;
            }
            Debug.WriteLine($"{_facAct}, {_townAct}, {_studyTypeAct}, {_studyYearAct}, {_fieldOfStudyDetailedAct}, {level.ToString()}");
            var json = DownloadJson(
                _facAct.Attributes["value"].Value,
                _townAct.Attributes["value"].Value,
                _studyTypeAct.Attributes["value"].Value,
                _studyYearAct.Attributes["value"].Value,               
                _fieldOfStudyDetailedAct.Attributes["value"].Value,
                ((int)level).ToString());
            var deserialized = JObject.Parse(json);
            if (deserialized["msg"] != null)
            {
                return;
            }

            var tab = ConvertJsonToHtmlNode(deserialized["plany"].ToString());
            ParseAndSaveCourses(tab.ChildNodes[1].ChildNodes["table"]);

            switch (level)
            {
                case ChangeLevel.FromDetailedFieldOfStudy:
                    break;
               
                case ChangeLevel.FromStudyYear:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    
                    break;
                case ChangeLevel.FromStudyType:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    
                    _selectStudyYear = ConvertJsonToHtmlNode(deserialized["r"].ToString());
                    break;
                case ChangeLevel.FromTown:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    
                    _selectStudyYear = ConvertJsonToHtmlNode(deserialized["r"].ToString());
                    _selectStudyType = ConvertJsonToHtmlNode(deserialized["m"].ToString());
                    break;
                case ChangeLevel.FromFaculty:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    
                    _selectStudyYear = ConvertJsonToHtmlNode(deserialized["r"].ToString());
                    _selectStudyType = ConvertJsonToHtmlNode(deserialized["m"].ToString());
                    _selectTown = ConvertJsonToHtmlNode(deserialized["t"].ToString());
                    break;
            }
        }

        private HtmlNode ConvertJsonToHtmlNode(string content)
        {
            var docc = new HtmlDocument();
            docc.OptionDefaultStreamEncoding = _encoding;
            docc.LoadHtml(content);
            return docc.DocumentNode;
        }

        private string DownloadJson(string f, string t, string m, string r,
                                             string z, string c)
        {
            string retJson = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = _encoding;
                retJson = wc.DownloadString($"{Url}?f={f}&t={t}&m={m}&r={r}&z={z}&c={c}");
                //example for getting json with subjects: vzdelavanie.uniza.sk/vzdelavanie/plany.php?f=1&t=Z&m=2&r=1&z=D1&c=6
            }
            return retJson;
        }

        private void ParseAndSaveCourses(HtmlNode htmlTable)
        {

            var rows = htmlTable.ChildNodes["tbody"].SelectNodes("tr");
            foreach (var x in rows)
            {
                var course = x.ChildNodes["td"]?.ChildNodes["a"];
                if (course != null)
                {
                    var crs = new CourseItem();
                    crs.Faculty = _facAct.InnerText;
                    crs.Town = _townAct.InnerText.Trim();
                    crs.YearOfStudy = _studyYearAct.InnerText;                    
                    crs.DetailedStudyOfField = _fieldOfStudyDetailedAct.InnerText;
                    crs.StudyType = _studyTypeAct.InnerText;
                    int spaceIdx = course.InnerText.IndexOf(" ");
                    crs.CourseCode = course.InnerText.Substring(0, spaceIdx);
                    crs.CourseName = course.InnerText.Substring(spaceIdx + 1, course.InnerText.Length - spaceIdx - 1);
                    crs.Short = x.ChildNodes[1].InnerText;
                    _allCourses.Add(crs);
                }
            }
        }
    }
}
