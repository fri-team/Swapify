using Microsoft.Extensions.Logging;

namespace FRITeam.Swapify.SwapifyBase.Emails
{
    public class RestorePasswordEmail : ConfirmationEmailBase
    {
        protected override string Subject => "Zabudnuté heslo na Swapify";
        protected override string PathToTemplate => @"Emails/EmailTemplates/RestorePasswordTemplate.html";

        public RestorePasswordEmail(ILoggerFactory loggerFactory, string sender, string senderDisplayName, string receiver, string baseUrl, string confirmationLink)
            : base(loggerFactory, sender, senderDisplayName, receiver, baseUrl, confirmationLink)
        {
        }
    }
}
