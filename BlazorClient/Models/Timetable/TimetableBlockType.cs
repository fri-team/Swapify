using System.Text.Json.Serialization;

namespace BlazorClient.Models.Timetable
{    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TimetableBlockType
    {
        Lecture = 1,
        Laboratory = 2,
        Excercise = 3
    }
}
