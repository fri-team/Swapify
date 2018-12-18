using System.Text;

namespace FRITeam.Swapify.Backend.Settings
{
    public abstract class SettingsBase : IValidatable
    {
        public virtual string ConfigFileName => "appsettings.json";
        public StringBuilder Errors { get; }

        protected SettingsBase()
        {
            Errors = new StringBuilder();
        }

        public abstract void Validate();
    }
}
