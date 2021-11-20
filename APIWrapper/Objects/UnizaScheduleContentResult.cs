using Newtonsoft.Json;
using System.Collections.Generic;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class UnizaScheduleContentResult
    {
        [JsonProperty("report")]
        public string Report { get; set; }
        [JsonProperty("ScheduleContent")]
        public List<ScheduleContent> ScheduleContents { get; set; }
    }
}
