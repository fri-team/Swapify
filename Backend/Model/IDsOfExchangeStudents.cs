using System;

namespace FRITeam.Swapify.Backend.Model
{
    public class IDsOfExchangeStudents
    {
        public string FirstID { get; set; }
        public string SecondID { get; set; }

        public IDsOfExchangeStudents(string first, string second)
        {
            FirstID = first;
            SecondID = second;
        }

        public bool Equals(IDsOfExchangeStudents other)
        {
            if ((other.FirstID == FirstID || other.FirstID == SecondID) &&
                (other.SecondID == FirstID || other.SecondID == SecondID))
            {
                return true;
            }

            return false;
        } 
    }
}
