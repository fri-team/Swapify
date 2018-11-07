using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace FRITeam.Swapify.Backend.Exceptions
{
    [Serializable]
    public class SettingException : Exception
    {
        public string ConfigFileName { get; set; }

        public SettingException(string configFileName, string message) : base(message)
        {
            ConfigFileName = configFileName;
        }

        protected SettingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ConfigFileName = info.GetString("ConfigFileName");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("ConfigFileName", ConfigFileName);
            base.GetObjectData(info, context);
        }
    }
}
