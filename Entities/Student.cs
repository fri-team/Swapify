using System;

namespace FRITeam.Swapify.Entities
{
    public class Student : BaseEntity
    {
        public Timetable Timetable { get; set; }
        public string PersonalNumber { get; set; }
        public Guid UserId { get; set; }
    }
}
