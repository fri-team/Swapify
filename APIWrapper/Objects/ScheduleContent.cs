using FRITeam.Swapify.APIWrapper.Enums;
using Newtonsoft.Json;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class ScheduleContent
    {
        private string _courseName;
        private int _day;

        [JsonProperty("t")]
        public string T { get; set; }
        [JsonProperty("u")]
        public string TeacherName { get; set; }
        [JsonProperty("r")]
        public string RoomName { get; set; }
        [JsonProperty("s")]        
        public string CourseName { get => _courseName; set => _courseName = ConvertFirstChar(value); }        
        [JsonProperty("k")]
        public string CourseCode { get; set; }
        [JsonProperty("p")]
        public string CourseShortcut { get; set; }
        [JsonProperty("tu")]
        public LessonType LessonType { get; set; }
        [JsonProperty("d")]
        public string D { get; set; }
        [JsonProperty("dw")]        
        public int Day { get => _day; set => _day = value - 1; }
        [JsonProperty("b")]
        public int BlockNumber { get; set; }
        [JsonProperty("tf ")]
        public string Tf { get; set; }
        [JsonProperty("tt")]
        public string Tt { get; set; }


        private static string ConvertFirstChar(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            else if (value.Length == 1)
                return value.ToUpper();
            else
                return char.ToUpper(value[0]) + value.Substring(1);
        }
    }
}
