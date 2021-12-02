using Newtonsoft.Json;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class CourseContent
    {
        [JsonProperty("value")]
        public string Code { get; set; }
        [JsonProperty("label")]
        public string Name { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; } 
    }
}
