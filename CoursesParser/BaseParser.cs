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
        private const string Url = "https://vzdelavanie.uniza.sk/vzdelavanie5/plany.php";
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
            _selectTown = _document.GetElementbyId("t");
            _selectStudyType = _document.GetElementbyId("m");
            _selectStudyYear = _document.GetElementbyId("r");
            _selectFieldOfStudy = _document.GetElementbyId("o");
            _selectFieldOfStudyDetailed = _document.GetElementbyId("z");
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
                            if (_selectFieldOfStudy != null)
                            {
                                foreach (var fieldOfStudy in _selectFieldOfStudy.ChildNodes)
                                {
                                    _fieldOfStudyAct = fieldOfStudy;
                                    Console.WriteLine(fieldOfStudy.InnerText);
                                    DownloadAndSaveCourses(ChangeLevel.FromFieldOfStudy);
                                    foreach (var fieldOfStudyDetailed in _selectFieldOfStudyDetailed.ChildNodes)
                                    {
                                        _fieldOfStudyDetailedAct = fieldOfStudyDetailed;
                                        DownloadAndSaveCourses(ChangeLevel.FromDetailedFieldOfStudy);
                                    }
                                }
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
                _fieldOfStudyAct?.Attributes["value"]?.Value == null ||
                _fieldOfStudyDetailedAct?.Attributes["value"]?.Value == null)
            {
                return;
            }
            Debug.WriteLine($"{_facAct}, {_townAct}, {_studyTypeAct}, {_studyYearAct}, {_fieldOfStudyAct}, {_fieldOfStudyDetailedAct}, {level.ToString()}");
            var json = DownloadJson(
                _facAct.Attributes["value"].Value,
                _townAct.Attributes["value"].Value,
                _studyTypeAct.Attributes["value"].Value,
                _studyYearAct.Attributes["value"].Value,
                _fieldOfStudyAct.Attributes["value"].Value,
                _fieldOfStudyDetailedAct.Attributes["value"].Value,
                ((int)level).ToString());
            var deserialized = JObject.Parse(json);
            if (deserialized["msg"] != null)
            {
                return;
            }

            var tab = ConvertJsonToHtmlNode(deserialized["plany"].ToString());
            ParseAndSaveCourses(tab.ChildNodes["table"]);

            switch (level)
            {
                case ChangeLevel.FromDetailedFieldOfStudy:
                    break;
                case ChangeLevel.FromFieldOfStudy:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    break;
                case ChangeLevel.FromStudyYear:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    _selectFieldOfStudy = ConvertJsonToHtmlNode(deserialized["o"].ToString());
                    break;
                case ChangeLevel.FromStudyType:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    _selectFieldOfStudy = ConvertJsonToHtmlNode(deserialized["o"].ToString());
                    _selectStudyYear = ConvertJsonToHtmlNode(deserialized["r"].ToString());
                    break;
                case ChangeLevel.FromTown:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    _selectFieldOfStudy = ConvertJsonToHtmlNode(deserialized["o"].ToString());
                    _selectStudyYear = ConvertJsonToHtmlNode(deserialized["r"].ToString());
                    _selectStudyType = ConvertJsonToHtmlNode(deserialized["m"].ToString());
                    break;
                case ChangeLevel.FromFaculty:
                    _selectFieldOfStudyDetailed = ConvertJsonToHtmlNode(deserialized["z"].ToString());
                    _selectFieldOfStudy = ConvertJsonToHtmlNode(deserialized["o"].ToString());
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
                                             string o, string z, string c)
        {
            string retJson = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = _encoding;
                retJson = wc.DownloadString($"{Url}?f={f}&t={t}&m={m}&r={r}&o={o}&z={z}&c={c}");
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
                    crs.Town = _townAct.InnerText;
                    crs.YearOfStudy = _studyYearAct.InnerText;
                    crs.StudyOfField = _fieldOfStudyAct.InnerText;
                    crs.DetailedStudyOfField = _fieldOfStudyDetailedAct.InnerText;
                    crs.StudyType = _studyTypeAct.InnerText;
                    int spaceIdx = course.InnerText.IndexOf(" ");
                    crs.CourseCode = course.InnerText.Substring(0, spaceIdx);
                    crs.CourseName = course.InnerText.Substring(spaceIdx + 1, course.InnerText.Length - spaceIdx - 1);

                    _allCourses.Add(crs);
                }
            }
        }
    }
}
