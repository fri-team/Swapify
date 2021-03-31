
using System;

namespace FRITeam.Swapify.Entities
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class Course : BaseEntity
    {
        /// <summary>
        /// Course code in specific format
        /// e.g. 5II210
        /// </summary>
        public string CourseCode { get; set; }        
        public string CourseName { get; set; }
        public Timetable Timetable { get; set; }
        public DateTime? LastUpdateOfTimetable { get; set; }
    }
}
