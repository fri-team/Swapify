using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper.Objects;

namespace FRITeam.Swapify.APIWrapper
{
    public interface ISchoolScheduleProxy
    {
        /// <summary>
        /// Return schedule for whole week by student number
        /// </summary>
        /// <param name="personalNumber">student number</param>
        Task<ScheduleTimetableResult> GetByPersonalNumber(string personalNumber);

        /// <summary>
        /// Return schedule for whole week by teacher unique number
        /// </summary>
        /// <param name="teacherNumber">teacher unique number</param>
        ScheduleTimetableResult GetByTeacherName(string teacherNumber);

        /// <summary>
        /// Return schedule for whole week by room number
        /// </summary>
        /// <param name="roomNumber">room number</param>
        ScheduleTimetableResult GetByRoomNumber(string roomNumber);

        /// <summary>
        /// Return schedule for whole week by subject code
        /// </summary>
        /// <param name="subjectCode">subject code</param>
        ScheduleTimetableResult GetBySubjectCode(string subjectCode, string yearOfStudy, string studyType);

        /// <summary>
        /// Return schedule for whole week from file for test purposes
        /// </summary>
        /// <param name="fileName">file name</param>
        ScheduleTimetableResult GetFromJsonFile(string fileName);
    }
}
