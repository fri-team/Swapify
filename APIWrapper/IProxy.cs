using APIWrapper.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIWrapper
{
    public interface IProxy
    {
        /// <summary>
        ///         Return schedule for whole week by study group
        /// </summary>
        /// <param name="studyGroupNumber">study group</param>
        ScheduleWeekContent GetScheduleContentByStudyGroup(string studyGroupNumber);

        /// <summary>
        ///         Return schedule for whole week by teacher unique number
        /// </summary>
        /// <param name="studyGroupNumber">teacher unique number</param>
        ScheduleWeekContent GetScheduleContentByTeacherName(string teacherNumber);

        /// <summary>
        ///         Return schedule for whole week by room number
        /// </summary>
        /// <param name="studyGroupNumber">room number</param>
        ScheduleWeekContent GetScheduleContentByRoomNumber(string roomNumber);

        /// <summary>
        ///         Return schedule for whole week by subject code
        /// </summary>
        /// <param name="studyGroupNumber">subject code</param>
        ScheduleWeekContent GetScheduleContentBySubjectCode(string subjectCode);
    }
}
