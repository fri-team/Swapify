using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class CourseContent
    {
        [JsonProperty("value")]
        public string Code { get; }
        [JsonProperty("label")]
        public string Name { get; }
        [JsonProperty("desc")]
        public string Description { get; }
        [JsonProperty("type")]
        public int Type { get; }

        public CourseContent(string code, string name, string description, int type)
        {
            Code = code;
            Name = name;
            Description = description;
            Type = type;
        }

        public bool IsSameAs(CourseContent content)
        {
            return Code.Equals(content.Code);
        }
    }
}
