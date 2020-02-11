using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class CourseContent
    {
        [JsonProperty("value")]
        public string ShortCut { get; }
        [JsonProperty("label")]
        public string Name { get; }
        [JsonProperty("desc")]
        public string Description { get; }
        [JsonProperty("type")]
        public int Type { get; }

        public CourseContent(string shortCut, string name, string description, int type)
        {
            ShortCut = shortCut;
            Name = name;
            Description = description;
            Type = type;
        }

        public bool IsSameAs(CourseContent content)
        {
            return ShortCut.Equals(content.ShortCut);
        }
    }
}
