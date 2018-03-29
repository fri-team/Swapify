using FRITeam.Swapify.APIWrapper.Objects;

namespace FRITeam.Swapify.APIWrapper
{
    public interface IProxySchedule
    {
        /// <summary>
        ///         Return schedule for whole week by study group
        /// </summary>
        /// <param name="studyGroupNumber">study group</param>
        ScheduleWeekContent GetByStudyGroup(string studyGroupNumber);

        /// <summary>
        ///         Return schedule for whole week by teacher unique number
        /// </summary>
        /// <param name="studyGroupNumber">teacher unique number</param>
        ScheduleWeekContent GetByTeacherName(string teacherNumber);

        /// <summary>
        ///         Return schedule for whole week by room number
        /// </summary>
        /// <param name="studyGroupNumber">room number</param>
        ScheduleWeekContent GetByRoomNumber(string roomNumber);

        /// <summary>
        ///         Return schedule for whole week by subject code
        /// </summary>
        /// <param name="studyGroupNumber">subject code</param>
        ScheduleWeekContent GetBySubjectCode(string subjectCode);
    }
}
