using FRITeam.Swapify.APIWrapper.Objects;

namespace FRITeam.Swapify.APIWrapper
{
    public interface ISchoolScheduleProxy
    {
        /// <summary>
        /// Return schedule for whole week by student number
        /// </summary>
        /// <param name="studentNumber">student number</param>
        ScheduleWeekContent GetByStudentNumber(string studentNumber);

        /// <summary>
        /// Return schedule for whole week by teacher unique number
        /// </summary>
        /// <param name="teacherNumber">teacher unique number</param>
        ScheduleWeekContent GetByTeacherName(string teacherNumber);

        /// <summary>
        /// Return schedule for whole week by room number
        /// </summary>
        /// <param name="roomNumber">room number</param>
        ScheduleWeekContent GetByRoomNumber(string roomNumber);

        /// <summary>
        /// Return schedule for whole week by subject code
        /// </summary>
        /// <param name="subjectCode">subject code</param>
        ScheduleWeekContent GetBySubjectCode(string subjectCode);
    }
}
