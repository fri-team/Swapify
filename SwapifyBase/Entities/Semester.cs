using System;

namespace FRITeam.Swapify.SwapifyBase.Entities
{
    public class Semester
    {
        public enum SemesterShortcut
        {
            Winter = 'Z',
            Summer = 'L'
        }
        public int Year { get; set; }
        public SemesterShortcut SemesterValue { get; set; }

        public static Semester GetSemester()
        {
            DateTime localDate = DateTime.Now;
            DateTime winterSemesterStart = new DateTime(localDate.Year, 9, 1); //start of winter semester 1.9.
            DateTime endOfTheYear = new DateTime(localDate.Year, 12, 31, 23, 59, 59); //end of the year
            DateTime newYear = new DateTime(localDate.Year, 1, 1); //end of the year
            DateTime winterSemesterEnd = new DateTime(localDate.Year, 2, 6); //end of winter semester and start of summer semester 15.2.
            DateTime summerSemesterEnd = new DateTime(localDate.Year, 7, 1); //end of summer semester 1.7.

            //if current semester is winter semester <1.9.; 31.12.>
            if (localDate.CompareTo(winterSemesterStart) != -1 && localDate.CompareTo(endOfTheYear) == -1)
                return new Semester
                {
                    Year = localDate.Year,
                    SemesterValue = SemesterShortcut.Winter
                };
            //if current semester is winter semester <1.1.; 15.2.)
            if (localDate.CompareTo(newYear) != -1 && localDate.CompareTo(winterSemesterEnd) == -1)
                return new Semester()
                {
                    Year = localDate.Year - 1,
                    SemesterValue = SemesterShortcut.Winter
                };
            //if current semester is summer semester <15.2.; 1.7.)
            if (localDate.CompareTo(winterSemesterEnd) != -1 && localDate.CompareTo(summerSemesterEnd) == -1)
                return new Semester()
                {
                    Year = localDate.Year - 1,
                    SemesterValue = SemesterShortcut.Summer
                };
            return null;
        }

        public override bool Equals(object obj)
        {
            return obj is Semester semester &&
                   Year == semester.Year &&
                   SemesterValue == semester.SemesterValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Year, SemesterValue);
        }
    }
}
