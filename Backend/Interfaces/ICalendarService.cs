
using FRITeam.Swapify.Entities;
using System.Text;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface ICalendarService
    {
        StringBuilder StartCalendar();

        StringBuilder CreateEvent(StringBuilder stringBuilder, Block block, Course course);

        StringBuilder EndCalendar(StringBuilder stringBuilder);
    }
}
