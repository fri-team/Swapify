using System.Collections.Generic;

namespace WebAPI.Models.TimetableModels
{
    public class Timetable
    {
        public List<TimetableBlock> Blocks { get; set; }
        public List<TimetableEvent> Events { get; set; }
    }
}
