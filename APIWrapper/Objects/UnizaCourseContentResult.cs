using Newtonsoft.Json;
using System.Collections.Generic;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class UnizaCourseContentResult
    {
        private bool _processed;
        private readonly Dictionary<int, CourseContent> _courseContentList;
        [JsonProperty("ScheduleTypeList")]
        private readonly Dictionary<string, dynamic> _scheduleTypeList;

        private const string Report = "report";
        public string Result { get; set; }        
        public Dictionary<int, CourseContent> CourseContentList { get { if (!_processed) Process(); return _courseContentList; } }
        public UnizaCourseContentResult()
        {
            _courseContentList = new();
            _processed = false;
        }
        private void Process()
        {            
            if (_scheduleTypeList != null && _scheduleTypeList.Count != 0 && !_processed)
            {
                if (_scheduleTypeList.ContainsKey(Report))
                {
                    object o = _scheduleTypeList[Report];
                    if (o != null)
                    {
                        Result = o.ToString();
                    }
                    _scheduleTypeList.Remove(Report);
                }                                
                foreach (var item in _scheduleTypeList)
                {
                    if (int.TryParse(item.Key, out int result))
                    {
                        _courseContentList.TryAdd(result, new CourseContent()
                        {
                            Code = item.Value.value,
                            Description = item.Value.desc,
                            Name = item.Value.label,
                            Type = item.Value.type
                        });
                    }
                }
                _processed = true;
            }            
        }
    }
}
