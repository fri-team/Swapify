using FRITeam.Swapify.APIWrapper.Enums;
using System.Collections.Generic;
using System.Linq;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class ScheduleHourContent
    {
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
        /// Subject shortcut
        /// </summary>
        public string CourseShortcut { get; }

        /// <summary>
        /// Full name of subject
        /// </summary>
        public string CourseName { get; }

        /// <summary>
        /// All study groups which have lesson in the same time
        /// </summary>
        public HashSet<string> StudyGroups { get; }

        /// <summary>
        /// Type of subject - compulsory, optional, compulsoryElective
        /// </summary>
        public SubjectType SubjectType { get; }

        public ScheduleHourContent(int blockNumber, bool isBlocked, LessonType lessonType,
            string teacherName, string roomName, string subjectShortcut, string subjectName,
            SubjectType subjectType, List<string> studyGroups)
        {
            BlockNumber = blockNumber;
            IsBlocked = isBlocked;
            LessonType = lessonType;
            TeacherName = teacherName;
            RoomName = roomName;
            CourseShortcut = subjectShortcut;
            CourseName = subjectName;
            SubjectType = subjectType;
            StudyGroups = new HashSet<string>();
            StudyGroups.UnionWith(studyGroups);
        }

        public bool IsSameBlockAs(ScheduleHourContent b2)
        {
            return (CourseName == b2?.CourseName) &&
                    (TeacherName == b2?.TeacherName) &&
                    (RoomName == b2?.RoomName) &&
                    (LessonType == b2?.LessonType) &&
                    (StudyGroups.SetEquals(b2?.StudyGroups));
        }

        public int GetIndexOfSameBlockInList(List<ScheduleHourContent> blocksInDay)
        {
            if (!blocksInDay.Any()) return -2;

            for (int curBlockIndex = 0; curBlockIndex < blocksInDay.Count; curBlockIndex++)
            {
                if (blocksInDay[curBlockIndex].IsSameBlockAs(this))
                {
                    return curBlockIndex;
                }
            }

            return -1;
        }
    }
}
