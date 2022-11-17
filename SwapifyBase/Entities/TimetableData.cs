using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using MongoDB.Driver;
using System;

namespace FRITeam.Swapify.SwapifyBase.Entities
{
    public class TimetableData : BaseEntity
    {
        public Timetable Timetable { get; set; }
        public string PersonalNumber { get; set; }
        public Guid UserId { get; set; }
        public bool ShowBlockedHours { get; set; }
        public TimetableType TimetableType {
            get
            {
                if (PersonalNumber.Length == 6) return TimetableType.StudentTimetable;
                else if (PersonalNumber.Length == 5) return TimetableType.TeacherTimetable;
                else return TimetableType.Unknown;
            }}
    }
}
