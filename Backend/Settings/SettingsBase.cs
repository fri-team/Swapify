using FRITeam.Swapify.Backend.Exceptions;
using System.Text;

namespace FRITeam.Swapify.Backend.Settings
{
    public abstract class SettingsBase : IValidatable
    {
        protected readonly StringBuilder _errors;

        protected SettingsBase()
        {
            _errors = new StringBuilder();
        }

        public abstract void Validate();

        protected void CheckErrors(string configName)
        {
            if (_errors.Length != 0)
            {
                throw new SettingException(configName, _errors.ToString());
            }
        }
    }
}
