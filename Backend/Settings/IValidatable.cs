using System.Text;

namespace FRITeam.Swapify.Backend.Settings
{
    public interface IValidatable
    {
        string ConfigFileName { get; }
        StringBuilder Errors { get; }
        void Validate();
    }
}
