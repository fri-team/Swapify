using BlazorClient.Models.Timetable;

namespace BlazorClient.ViewModels
{
    public class TimetableBlockViewModel
    {
        private TimetableBlockModel _timetableBlock;

        public TimetableBlockViewModel(TimetableBlockModel timetableBlock)
        {
            _timetableBlock = timetableBlock;
        }

        public string CourseShortcut => _timetableBlock.CourseShortcut;
        public string Room => _timetableBlock.Room;
        public string Teacher => _timetableBlock.Teacher;
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public double MarginTop { get; set; }

        public void OnClick()
        {
            // TODO
        }
    }
}
