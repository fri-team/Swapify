using FRITeam.Swapify.Backend.Exceptions;
using System.Text;

namespace FRITeam.Swapify.Backend.Settings
{
    public abstract class SettingsBase : IValidatable
    {
        protected StringBuilder Errors { get; }

        protected SettingsBase()
        {
            Errors = new StringBuilder();
        }

        public abstract void Validate();

        protected void CheckErrors(string configName)
        {
            if (Errors.Length != 0)
            {
                throw new SettingException(configName, Errors.ToString());
            }
        }
    }
}
