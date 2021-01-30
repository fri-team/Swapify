using FRITeam.Swapify.APIWrapper.Enums;
using System.Collections.Generic;
using System.Linq;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class ScheduleHourContent
    {
        /// <summary>
        /// from 1 (monday) to 5 (friday)
        /// </summary>
        public int Day { get; }

        /// <summary>
        /// Every day has 13 blocks(1-13)
        /// block 1 starts at 7:00
        /// block 13 starts at 19:00
        /// </summary>
        public int BlockNumber { get; }

        /// <summary>
        /// Is block blocked?
        /// </summary>
        public bool IsBlocked { get; }

        /// <summary>
        /// Type of lesson - lecture, laboratory, excersise
        /// </summary>
        public LessonType LessonType { get; }

        /// <summary>
        /// Name of the teacher
        /// </summary>
        public string TeacherName { get; }

        /// <summary>
        /// Room number where is lesson
        /// </summary>
        public string RoomName { get; }

        /// <summary>
        /// Subject code
        /// </summary>
        public string CourseCode { get; }

        /// <summary>
        /// Full name of subject
        /// </summary>
        public string CourseName { get; }
        /// <summary>
        /// Type of subject - compulsory, optional, compulsoryElective
        /// </summary>
        public SubjectType SubjectType { get; }

        public ScheduleHourContent(int day, int blockNumber, bool isBlocked, LessonType lessonType,
            string teacherName, string roomName, string subjectCode, string subjectName,
            SubjectType subjectType)
        {
            Day = day;
            BlockNumber = blockNumber;
            IsBlocked = isBlocked;
            LessonType = lessonType;
            TeacherName = teacherName;
            RoomName = roomName;
            CourseCode = subjectCode;
            CourseName = subjectName;            
            SubjectType = subjectType;
        }
        
        public bool IsSameBlockAs(ScheduleHourContent b2)
        {
            return (CourseName == b2?.CourseName) &&
                    (TeacherName == b2?.TeacherName) &&
                    (RoomName == b2?.RoomName) &&
                    (LessonType == b2?.LessonType);
        }

        
    }
}
