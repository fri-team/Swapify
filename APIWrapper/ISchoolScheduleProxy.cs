using System.Collections.Generic;
using FRITeam.Swapify.APIWrapper.Objects;

namespace FRITeam.Swapify.APIWrapper
{
    public interface ISchoolScheduleProxy
    {
        /// <summary>
        /// Return schedule for whole week by study group
        /// </summary>
        /// <param name="studyGroupNumber">study group</param>
        IEnumerable<ScheduleHourContent> GetByStudyGroup(string studyGroupNumber);

        /// <summary>
        /// Return schedule for whole week by teacher unique number
        /// </summary>
        /// <param name="teacherNumber">teacher unique number</param>
        IEnumerable<ScheduleHourContent> GetByTeacherName(string teacherNumber);

        /// <summary>
        /// Return schedule for whole week by room number
        /// </summary>
        /// <param name="roomNumber">room number</param>
        IEnumerable<ScheduleHourContent> GetByRoomNumber(string roomNumber);

        /// <summary>
        /// Return schedule for whole week by subject code
        /// </summary>
        /// <param name="subjectCode">subject code</param>
        IEnumerable<ScheduleHourContent> GetBySubjectCode(string subjectCode);
    }
}
