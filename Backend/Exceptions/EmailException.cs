using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace FRITeam.Swapify.Backend.Exceptions
{
    [Serializable]
    public class EmailException : Exception
    {
        public string EmailType { get; set; }

        public EmailException(string emailType, string message) : base(message)
        {
            EmailType = emailType;
        }

        protected EmailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            EmailType = info.GetString("EmailType");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("EmailType", EmailType);
            base.GetObjectData(info, context);
        }
    }
}
