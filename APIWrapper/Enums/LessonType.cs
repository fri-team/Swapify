using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace FRITeam.Swapify.APIWrapper.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LessonType
    {
        [EnumMember(Value = "")]     
        Blocked = 0,
        [EnumMember(Value = "L")]
        Laboratory = 1,
        [EnumMember(Value = "C")]
        Excercise = 2,
        [EnumMember(Value = "P")]
        Lecture = 3,
        [EnumMember(Value = "E")]
        Event = 5
    }
}
