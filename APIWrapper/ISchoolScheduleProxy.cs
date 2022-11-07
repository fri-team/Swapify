using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;

namespace FRITeam.Swapify.APIWrapper
{
    public interface ISchoolScheduleProxy
    {
        /// <summary>
        /// Return schedule for whole week by student number
        /// </summary>
        /// <param name="personalNumber">student number</param>
        Task<ScheduleTimetableResult> GetByPersonalNumber(string personalNumber, UserType userType = UserType.Student);

        /// <summary>
        /// Return schedule for whole week by teacher unique number
        /// </summary>
        /// <param name="teacherNumber">teacher unique number</param>
        Task<ScheduleTimetableResult> GetByTeacherName(string teacherNumber);

        /// <summary>
        /// Return schedule for whole week by room number
        /// </summary>
        /// <param name="roomNumber">room number</param>
        Task<ScheduleTimetableResult> GetByRoomNumber(string roomNumber);

        /// <summary>
        /// Return schedule for whole week by subject code
        /// </summary>
        /// <param name="subjectCode">subject code</param>
        Task<ScheduleTimetableResult> GetBySubjectCode(string subjectCode, string yearOfStudy, string studyType);

        /// <summary>
        /// Return schedule for whole week from file for test purposes
        /// </summary>
        /// <param name="fileName">file name</param>
        ScheduleTimetableResult GetFromJsonFile(string fileName);
    }
}
