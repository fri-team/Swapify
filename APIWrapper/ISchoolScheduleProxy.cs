using System.Collections.Generic;
using FRITeam.Swapify.APIWrapper.Objects;

namespace FRITeam.Swapify.APIWrapper
{
    public interface ISchoolScheduleProxy
    {
        /// <summary>
        /// Return schedule for whole week by student number
        /// </summary>
        /// <param name="personalNumber">student number</param>
        ScheduleTimetable GetByPersonalNumber(string personalNumber);

        /// <summary>
        /// Return schedule for whole week by teacher unique number
        /// </summary>
        /// <param name="teacherNumber">teacher unique number</param>
        ScheduleTimetable GetByTeacherName(string teacherNumber);

        /// <summary>
        /// Return schedule for whole week by room number
        /// </summary>
        /// <param name="roomNumber">room number</param>
        ScheduleTimetable GetByRoomNumber(string roomNumber);

        /// <summary>
        /// Return schedule for whole week by subject code
        /// </summary>
        /// <param name="subjectCode">subject code</param>
        ScheduleTimetable GetBySubjectCode(string subjectCode, string yearOfStudy, string studyType);

        /// <summary>
        /// Return schedule for whole week from file for test purposes
        /// </summary>
        /// <param name="fileName">file name</param>
        ScheduleTimetable GetFromJsonFile(string fileName);
    }
}
