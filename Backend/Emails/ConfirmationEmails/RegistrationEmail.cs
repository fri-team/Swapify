using Microsoft.Extensions.Logging;

namespace FRITeam.Swapify.Backend.Emails
{
    public class RegistrationEmail : ConfirmationEmailBase
    {
        protected override string Subject => "Registrácia na Swapify";
        protected override string PathToTemplate => @"Emails/EmailTemplates/RegistrationTemplate.html";

        public RegistrationEmail(ILoggerFactory loggerFactory, string sender, string senderDisplayName, string receiver, string baseUrl, string confirmationLink)
            : base(loggerFactory, sender, senderDisplayName, receiver, baseUrl, confirmationLink)
        {
        }
    }
}
