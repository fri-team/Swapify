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
        public byte StartColumn { get; set; }
        public byte EndColumn { get; set; }
        public byte StartRow { get; set; }
        public byte EndRow { get; set; }
        public bool MarginTop { get; set; }

        public void OnClick()
        {
            // TODO
        }
    }
}
