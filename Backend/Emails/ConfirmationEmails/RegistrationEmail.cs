
using System.Net.Mail;

namespace FRITeam.Swapify.Backend.Emails.ConfirmationEmails
{
    public class RegistrationEmail : ConfirmationEmailBase
    {
        public override string Subject => "Registrácia na Swapify";
        protected override string PathToTemplate => @"Emails/EmailTemplates/RegistrationTemplate.html";

        public RegistrationEmail(string sender, string senderDisplayName, string receiver)
            : base(sender, senderDisplayName, receiver)
        {
        }
    }
}
