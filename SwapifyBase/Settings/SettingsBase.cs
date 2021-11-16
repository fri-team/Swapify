using System;
using System.Reflection;
using System.Text;

namespace FRITeam.Swapify.SwapifyBase.Settings
{
    public abstract class SettingsBase : IValidatable
    {
        public virtual string ConfigFileName => "appsettings.json";
        public StringBuilder Errors { get; }

        protected SettingsBase()
        {
            Errors = new StringBuilder();
        }        

        public void Validate()
        {
            foreach (PropertyInfo info in GetType().GetProperties())
            {
                Type propertyType = Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType;
                object propertyValue = info.GetValue(this);
                if (propertyValue == null)
                {
                    Errors.AppendLine($"Setting {info.Name} is missing in {GetType().Name} configuration section.");
                }
                else
                {
                    if (propertyType == typeof(string))
                    {
                        var stringValue = (string)propertyValue;
                        stringValue = stringValue.Replace(" ", "");
                        if (string.IsNullOrEmpty(stringValue))
                            Errors.AppendLine($"Setting {info.Name} in {GetType().Name} configuration section has empty value.");
                    }
                }
            }
            if (Errors.Length == 0)
                ValidateInternal();
        }

        protected virtual void ValidateInternal()
        {
            //override if any extra validation is needed
        }
    }
}
