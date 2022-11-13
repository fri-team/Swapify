using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using MongoDB.Driver;
using System;

namespace FRITeam.Swapify.SwapifyBase.Entities
{
    public class UserData : BaseEntity
    {
        public Timetable Timetable { get; set; }
        public string PersonalNumber { get; set; }
        public Guid UserId { get; set; }

        public UserType GetUserType()
        {
            if (PersonalNumber.Length == 6) return UserType.Student;
            else if (PersonalNumber.Length == 5) return UserType.Teacher;
            else throw new ArgumentException("Users personal number has wrong format");
        }
    }
}
