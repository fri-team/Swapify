namespace WebAPI.Models
{
    public class TimetableBlock
    {
        public int Day { get; set; }
        public int StartBlock { get; set; }
        public int EndBlock { get; set; }
        public string CourseName { get; set; }
        public string CourseShortcut { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }
        public TimetableBlockType Type { get; set; }
    }
}
