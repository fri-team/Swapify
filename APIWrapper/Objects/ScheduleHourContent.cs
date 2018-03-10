using APIWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIWrapper.Objects
{
    public class ScheduleHourContent
    {
        /// <summary>
        ///         Every day has 13 blocks(1-13)
        ///         block 1 starts at 7:00
        ///         block 13 starts at 19:00
        /// </summary>
        public int BlockNumber { get; internal set; }

        /// <summary>
        ///         Is block blocked?
        /// </summary>
        public bool IsBlocked { get; internal set; }

        /// <summary>
        ///         Type of lesson - lecture, laboratory, excersise
        /// </summary>
        public ELessonType LessonType { get; internal set; }

        /// <summary>
        ///         Name of the teacher
        /// </summary>
        public string TeacherName { get; internal set; }

        /// <summary>
        ///         Room number where is lesson
        /// </summary>
        public string RoomName { get; internal set; }

        /// <summary>
        ///         Subject shortcut
        /// </summary>
        public string SubjectShortcut { get; internal set; }

        /// <summary>
        ///         Full name of subject
        /// </summary>
        public string SubjectName { get; internal set; }

        /// <summary>
        ///         All study groups which have lesson in the same time
        /// </summary>
        public List<string> StudyGroups { get; internal set; }

        /// <summary>
        ///         Type of subject - compulsory, optional, compulsoryElective
        /// </summary>
        public ESubjectType SubjectType { get; internal set; }

        /// <summary>
        ///         Block is empty (free block)
        /// </summary>
        public bool IsEmptyBlock { get; internal set; }

        public ScheduleHourContent()
        {
            StudyGroups = new List<string>();
        }
    }
}
