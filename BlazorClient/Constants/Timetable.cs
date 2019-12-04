using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorClient.Constants
{
    public static class Timetable
    {
        public static string[] DAYS = new string[] { "Po", "Ut", "St", "Št", "Pi" };
        public static string[] HOURS = new string[] {
            "07:00", "08:00", "09:00", "10:00", "11:00",
            "12:00", "13:00", "14:00", "15:00", "16:00",
            "17:00", "18:00", "19:00"};
    }
}
