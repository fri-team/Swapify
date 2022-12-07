using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebAPI.Models.TimetableModels
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum TimetableBlockType
    {
        Lecture = 1,
        Laboratory = 2,
        Excercise = 3,
        Blocked = 4
    }
}
