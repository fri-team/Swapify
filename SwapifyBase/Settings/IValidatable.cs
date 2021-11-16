using System.Text;

namespace FRITeam.Swapify.SwapifyBase.Settings
{
    public interface IValidatable
    {
        string ConfigFileName { get; }
        StringBuilder Errors { get; }
        void Validate();
    }
}
