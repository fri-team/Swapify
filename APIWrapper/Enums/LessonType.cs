using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace FRITeam.Swapify.APIWrapper.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LessonType
    {
        [EnumMember(Value = "A")]     
        None = 0,
        Laboratory = 1,
        Excercise = 2,
        Lecture = 3
    }
}
