using CoursesParser.Enums;
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
        private string _url = "http://vzdelavanie.uniza.sk/vzdelavanie/plany.php";
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
        private HashSet<string> _allCourses;
        private Encoding _encoding;

        public BaseParser()
        {
            _encoding = CodePagesEncodingProvider.Instance.GetEncoding(1250);
            
            var web = new HtmlWeb();
            web.AutoDetectEncoding = false;

            web.OverrideEncoding = _encoding;

            _allCourses = new HashSet<string>(5000);
            _document = web.Load(_url);
            _document.OptionDefaultStreamEncoding = _encoding;

            _selectFaculties = _document.GetElementbyId("f");
            _selectTown = _document.GetElementbyId("t");
            _selectStudyType = _document.GetElementbyId("m");
            _selectStudyYear = _document.GetElementbyId("r");
            _selectFieldOfStudy = _document.GetElementbyId("o");
            _selectFieldOfStudyDetailed = _document.GetElementbyId("z");
            _selectPlansTable = _document.GetElementbyId("plany").ChildNodes["table"];

        }


        public HashSet<string> ParseFaculties()
        {
            
            foreach (var facultyOption in _selectFaculties.ChildNodes)
            {
                _facId = facultyOption.Attributes["value"].Value;
                DownloadAndSaveCourses(ChangeLevel.FromFaculty);
                foreach (var town in _selectTown.ChildNodes)
                {
                    _townId = town.Attributes["value"].Value;
                    DownloadAndSaveCourses(ChangeLevel.FromTown);
                    foreach(var studyType in _selectStudyType.ChildNodes)
                    {
                        _studyTypeId = studyType.Attributes["value"].Value;
                        DownloadAndSaveCourses(ChangeLevel.FromStudyType);
                        foreach (var studyYear in _selectStudyYear.ChildNodes)
                        {
                            _studyYearId = studyYear.Attributes["value"].Value;
                            DownloadAndSaveCourses(ChangeLevel.FromStudyYear);
                            foreach (var fieldOfStudy in _selectFieldOfStudy.ChildNodes)
                            {
                                _fieldOfStudyId = fieldOfStudy.Attributes["value"].Value;
                                DownloadAndSaveCourses(ChangeLevel.FromFieldOfStudy);
                                foreach (var fieldOfStudyDetailed in _selectFieldOfStudyDetailed.ChildNodes)
                                {
                                    _fieldOfStudyDetailedId = fieldOfStudyDetailed.Attributes["value"].Value;
                                    DownloadAndSaveCourses(ChangeLevel.FromDetailedFieldOfStudy);
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
            if (_facId == null || _townId == null || _studyTypeId == null || _studyYearId == null ||
                _fieldOfStudyId == null || _fieldOfStudyDetailedId == null)
            {
                return;
            }
            Debug.WriteLine($"{_facId}, {_townId}, {_studyTypeId}, {_studyYearId}, {_fieldOfStudyId}, {_fieldOfStudyDetailedId}, {level.ToString()}");
            var json = DownloadJson(_facId, _townId, _studyTypeId, _studyYearId,
                                          _fieldOfStudyId, _fieldOfStudyDetailedId, ((int)level).ToString());
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

        private string DownloadJson(string f,string t,string m,string r,
                                             string o,string z,string c)
        {
            string retJson = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = _encoding;
                retJson = wc.DownloadString($"{_url}?f={f}&t={t}&m={m}&r={r}&o={o}&z={z}&c={c}");
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
                    _allCourses.Add(course.InnerText);
                }
            }
        }

        public Tuple<string,string> SplitCodeAndName(string course)
        {
            Tuple<string, string> ret;
            int spaceIdx = course.IndexOf(" ");
            ret = new Tuple<string, string>(course.Substring(0, spaceIdx),
                  course.Substring(spaceIdx + 1, course.Length - spaceIdx - 1));
            return ret;
        }

    }
}
